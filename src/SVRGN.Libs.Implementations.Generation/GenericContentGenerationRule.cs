using SVRGN.Libs.Contracts.Generation;
using SVRGN.Libs.Contracts.Service.Logging;
using SVRGN.Libs.Contracts.Service.Object;
using SVRGN.Libs.Contracts.TextParsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVRGN.Libs.Implementations.Generation
{
    public class GenericContentGenerationRule : IContentGenerationRule
    {
        #region Properties

        public string Name { get; private set; }

        public bool IsSyntacticallyOk { get; private set; } = false;

        public List<string> Conditions { get; private set; }

        public string Order { get; private set; }

        public string Choice { get; private set; }
        public List<IContentGenerationRuleLogicItem> LogicItems { get; private set; }

        private string originalText;

        private ILogService logService;
        private ITextParsingService textParsingService;
        private IObjectService objectService;

        #endregion Properties

        #region Construction

        public GenericContentGenerationRule(string Name, string Text, ITextParsingService TextParsingService, ILogService LogService, IObjectService objectService)
        {
            this.Name = Name;

            textParsingService = TextParsingService;
            logService = LogService;
            this.objectService = objectService;

            originalText = Text;

            Conditions = new List<string>();

            IsSyntacticallyOk = this.ParseText(Text);
        }
        #endregion Construction

        #region Methods

        #region ParseText
        private bool ParseText(string Text)
        {
            bool result = false;

            logService.Debug("GenericContentGenerationRule", "ParseText", $"Attempting to parse the text '{Text}'.");

            int minSentenceLength = 5;

            string[] words = Text.Split(' ', ',', ';');

            if (words[0].ToLower().Contains("randomize"))  // TODO: find a more elegant way to deal with this command
            {
                string editedText = Text;
                Order = textParsingService.ExtractOrderFromPrimitiveText(editedText);
                Choice = textParsingService.ExtractChoiceFromPrimitiveText(editedText);
                result = true;
            }
            else
            {
                if (words.Length <= minSentenceLength)
                {
                    logService.Warning("GenericContentGenerationRule", "ParseText", $"The text '{Text}' does not fit the minimum word count of '{minSentenceLength}' and is therefore not parsed!");
                }
                else
                {
                    try
                    {
                        string editedText = Text;
                        bool hasCondition = textParsingService.HasCondition(Text);
                        bool isOk = true;

                        if (hasCondition)
                        {
                            this.Conditions = textParsingService.ExtractConditions(Text, out editedText);
                        }

                        //continue regular parsing
                        this.Order = textParsingService.ExtractOrder(editedText);
                        this.Choice = textParsingService.ExtractChoice(editedText);
                        this.LogicItems = ParseLogicFromText(textParsingService.ExtractLogic(editedText));

                        if (LogicItems != null && LogicItems.Count.Equals(0))
                        {
                            //should have logic items (list is created) but does not - not syntactically ok!
                            isOk = false;
                        }

                        result = isOk;
                    }
                    catch (Exception ex)
                    {
                        logService.Warning("GenericContentGenerationRule", "ParseText", $"Could not parse the text '{Text}' due to an Exception!", ex);
                    }
                }
            }

            if (result)
            {
                logService.Debug("GenericContentGenerationRule", "ParseText", $"Successfully parsed the text '{Text}'.");
            }
            else
            {
                logService.Warning("GenericContentGenerationRule", "ParseText", $"Could not parse the text '{Text}'!");
            }

            return result;
        }
        #endregion ParseText

        #region ParseLogicFromText
        private List<IContentGenerationRuleLogicItem> ParseLogicFromText(string Input)
        {
            List<IContentGenerationRuleLogicItem> result = default;

            logService.Debug("GenericContentGenerationRule", "ParseLogicFromText", $"Attempting to parse '{Input}'");

            if (!string.IsNullOrWhiteSpace(Input))
            {
                result = new List<IContentGenerationRuleLogicItem>();
                IList<string> logicItems = textParsingService.SplitLogicItems(Input);
                if (logicItems.Count > 0)
                {
                    logService.Debug("GenericContentGenerationRule", "ParseLogicFromText", $"Found '{logicItems.Count}' Logic Items applicable");
                    foreach (string logicItemline in logicItems)
                    {
                        string[] logicItemlineParts = logicItemline.Split('|');
                        IContentGenerationRuleLogicItem newItem = objectService.Create<IContentGenerationRuleLogicItem>(logicItemlineParts[0], logicItemlineParts[1]);
                        result.Add(newItem);
                    }
                }
                else
                {
                    logService.Warning("GenericContentGenerationRule", "ParseLogicFromText", $"Logic Item text is not empty ('{Input}') but no items could be parsed - is the syntax correct?");
                }
            }

            return result;
        }
        #endregion ParseLogicFromText

        #region GetOriginalText
        public string GetOriginalText()
        {
            return originalText;
        }
        #endregion GetOriginalText

        #endregion Methods

        #region Events

        #endregion Events
    }
}
