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
    public class ContentGenerationService : IContentGenerationService
    {
        #region Properties

        private IObjectService objectService;
        private ILogService logService;

        #endregion Properties

        #region Construction

        public ContentGenerationService(ILogService LogService, IObjectService objectService)
        {
            this.logService = LogService;
            this.objectService = objectService;
        }

        #endregion Construction

        #region Methods

        #region CreateGenerator
        public IContentGenerator CreateGenerator(string Name)
        {
            IContentGenerator result = default;

            result = objectService.Create<ContentGenerator>(Name);

            return result;
        }
        #endregion CreateGenerator

        #endregion Methods

        #region Events

        #endregion Events
    }
}
