using SVRGN.Libs.Contracts.Generation;
using SVRGN.Libs.Contracts.Service.Logging;
using SVRGN.Libs.Contracts.Service.Object;
using SVRGN.Libs.Contracts.Service.TextParsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVRGN.Libs.Implementations.Generation
{
    public abstract class BaseStep : IContentGenerationStep
    {
        #region Properties

        public string Name { get; private set; }

        public int Order { get; set; }

        public bool HasBeenUsed { get; internal set; } = false;

        protected ILogService logService;
        protected IObjectService objectService;
        protected ITextParsingService textParsingService;

        protected Random random;

        #endregion Properties

        #region Construction

        public BaseStep(string Name, int Order, ILogService LogService, IObjectService ObjectService, ITextParsingService TextParsingService)
        {
            this.Name = Name;
            this.Order = Order;
            logService = LogService;
            objectService = ObjectService;
            textParsingService = TextParsingService;
            random = new Random();
        }

        #endregion Construction

        #region Methods

        #region Process
        public abstract IContentGenerationResult<T> Process<T>(IContentGenerationResult<T> PreviousResult, IContentGenerationRule[] Rules, params object[] Parameters) where T : IEquatable<T>, ICloneable;
        #endregion Process

        #endregion Methods

        #region Events

        #endregion Events
    }
}
