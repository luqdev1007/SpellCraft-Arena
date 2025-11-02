using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Project.Develop.Runtime.UI.Core.TestPopup
{
    public class ConfirmPopupPresenter : PopupPresenterBase
    {
        private readonly ConfirmPopupView _view;

        public ConfirmPopupPresenter(
            ConfirmPopupView view, 
            ICoroutinesPerformer coroutinesPerformer) : base (coroutinesPerformer)
        {
            _view = view;
        }

        protected override PopupViewBase PopupView => _view;

        public override void Initialize()
        {
            base.Initialize();

            _view.SetText("Вы уверены?");
        }
    }
}
