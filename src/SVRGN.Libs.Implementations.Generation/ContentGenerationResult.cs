using SVRGN.Libs.Contracts.Base;
using SVRGN.Libs.Contracts.Generation;
using SVRGN.Libs.Contracts.Service.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVRGN.Libs.Implementations.Generation
{
    public class ContentGenerationResult<T> : IContentGenerationResult<T>
    {
        #region Properties

        public string Name { get; private set; }

        public bool IsFinal { get; private set; }

        public T Value { get; private set; }

        public IList<IError> Errors { get; private set; }

        private ILogService logService;

        #endregion Properties

        #region Construction

        public ContentGenerationResult(string Name, ILogService LogService)
        {
            this.Name = Name;
            logService = LogService;

            IsFinal = false;
            Value = default;
            Errors = new List<IError>();
        }

        #endregion Construction

        #region Methods

        #region AddError
        public void AddError(IError Error)
        {
            Errors.Add(Error);
        }
        #endregion AddError

        #region SetFinalized
        public void SetFinalized()
        {
            IsFinal = true;
        }
        #endregion SetFinalized

        #region SetValue
        public void SetValue(T NewValue)
        {
            Value = NewValue;
        }
        #endregion SetValue

        #endregion Methods

        #region Events

        #endregion Events
    }
}
