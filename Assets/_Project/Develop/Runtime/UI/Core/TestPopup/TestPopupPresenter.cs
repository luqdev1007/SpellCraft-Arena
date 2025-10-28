using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Project.Develop.Runtime.UI.Core.TestPopup
{
    public class TestPopupPresenter : PopupPresenterBase
    {
        private readonly TestPopupView _view;

        public TestPopupPresenter(TestPopupView view, ICoroutinesPerformer coroutinesPerformer) : base (coroutinesPerformer)
        {
            _view = view;
        }

        protected override PopupViewBase PopupView => _view;

        public override void Initialize()
        {
            base.Initialize();

            _view.SetText("ВАС ВЫ*БАЛИ");
        }
    }
}
