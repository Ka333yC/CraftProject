using Assets.Scripts.Core.UICore.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Assets.Scripts.Core.UICore.PopUp
{
	public class PopUpViewStack : IEnumerable<BasePopUpView>
	{
		public readonly BasePageView ParentPage;

		private readonly LinkedList<BasePopUpView> _viewStack = new LinkedList<BasePopUpView>();

		public PopUpViewStack(BasePageView parentPage)
		{
			ParentPage = parentPage;
		}

		public void OpenView(BasePopUpView viewToOpen)
		{
			_viewStack.AddLast(viewToOpen);
			viewToOpen.Stack = this;
			viewToOpen.Open();
		}

		public void CloseLastView()
		{
			var viewToClose = _viewStack.Last();
			_viewStack.RemoveLast();
			viewToClose.Close();
		}

		public IEnumerator<BasePopUpView> GetEnumerator()
		{
			return _viewStack.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
