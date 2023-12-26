using SVRGN.Libs.Contracts.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVRGN.Libs.Implementations.Generation
{
    public class ContentGenerationRuleLogicItem : IContentGenerationRuleLogicItem
    {
        #region Properties

        public string Prerequisite { get; private set; }

        public string Consequence { get; private set; }

        #endregion Properties

        #region Construction

        public ContentGenerationRuleLogicItem(string Prerequisite, string Consequence)
        {
            this.Prerequisite = Prerequisite;
            this.Consequence = Consequence;
        }

        #endregion Construction

        #region Methods

        #endregion Methods

        #region Events

        #endregion Events
    }
}
