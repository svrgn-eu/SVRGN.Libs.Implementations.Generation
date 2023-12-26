using SVRGN.Libs.Contracts.Generation;
using SVRGN.Libs.Contracts.Service.Logging;
using SVRGN.Libs.Contracts.Service.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVRGN.Libs.Implementations.Generation
{
    public class ContentGenerator : IContentGenerator
    {
        #region Properties

        public string Name { get; private set; }

        private List<IContentGenerationRule> rules;
        private List<IContentGenerationStep> steps;

        private ILogService logService;
        private IObjectService objectService;

        #endregion Properties

        #region Construction

        public ContentGenerator(string Name, ILogService LogService, IObjectService objectService)
        {
            this.Name = Name;
            logService = LogService;
            this.objectService = objectService;

            rules = new List<IContentGenerationRule>();
            steps = new List<IContentGenerationStep>();
        }

        #endregion Construction

        #region Methods

        #region AddRule: adds a rule if a rule with the same name does not yet exist
        /// <summary>
        /// adds a rule if a rule with the same name does not yet exist
        /// </summary>
        /// <param name="NewRule">the new rule to be added</param>
        /// <returns>true, if the rule did not exists previously and there was no error adding it</returns>
        public bool AddRule(IContentGenerationRule NewRule)
        {
            bool result = false;

            try
            {
                if (!rules.Any(x => x.Name.Equals(NewRule.Name)))
                {
                    rules.Add(NewRule);
                    result = true;
                    logService.Debug("ContentGenerator", "AddRule", $"Successfully added rule '{NewRule.Name}' to ContentGenerator '{Name}'");
                }
                else
                {
                    logService.Error("ContentGenerator", "AddRule", $"Could not add rule '{NewRule.Name}' as a rule with the same name does already exist in ContentGenerator '{Name}'!");
                }
            }
            catch (Exception ex)
            {
                logService.Error("ContentGenerator", "AddRule", $"Could not add rule '{NewRule.Name}' to ContentGenerator '{Name}'", ex);
            }

            return result;
        }
        #endregion AddRule

        #region AddStep
        public bool AddStep(IContentGenerationStep NewStep)
        {
            bool result = false;

            try
            {
                if (!steps.Any(x => x.Name.Equals(NewStep.Name)))
                {
                    steps.Add(NewStep);
                    result = true;
                    logService.Debug("ContentGenerator", "AddStep", $"Successfully added step '{NewStep.Name} (Order: {NewStep.Order})' to ContentGenerator '{Name}'");
                }
                else
                {
                    logService.Error("ContentGenerator", "AddStep", $"Could not add step '{NewStep.Name}' as a step with the same name does already exist in ContentGenerator '{Name}'!");
                }
            }
            catch (Exception ex)
            {
                logService.Error("ContentGenerator", "AddStep", $"Could not add step '{NewStep.Name}' to ContentGenerator '{Name}'", ex);
            }

            return result;
        }
        #endregion AddStep

        #region Generate
        public IContentGenerationResult<T> Generate<T>(params object[] Parameters) where T : IEquatable<T>, ICloneable
        {
            IContentGenerationResult<T> result = default;

            result = objectService.Create<ContentGenerationResult<T>>($"ContentGenerationResult from '{Name}' Created@ '{DateTime.Now}'");

            foreach (IContentGenerationStep step in steps.OrderBy(x => x.Order))  // follow all steps in order - chain of command implementation
            {
                result = step.Process(result, rules.ToArray(), Parameters);
            }

            result.SetFinalized();  //mark result as final after all steps have been passed

            return result;
        }
        #endregion Generate

        #region GetNumberOfSteps
        public int GetNumberOfSteps()
        {
            return steps.Count();
        }
        #endregion GetNumberOfSteps

        #endregion Methods

        #region Events

        #endregion Events
    }
}
