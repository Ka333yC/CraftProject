using Assets.Scripts.Core.UICore.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Core.UICore
{
	public class PageViewStack : IEnumerable<BasePageView>
	{
		private readonly LinkedList<BasePageView> _viewStack = new LinkedList<BasePageView>();

		public void OpenView(BasePageView viewToOpen)
		{
			// Скрываем последнюю вью
			var viewToMoveOut = _viewStack.LastOrDefault();
			viewToMoveOut?.MoveOut();
			// Открываем новую вью перед последней
			_viewStack.AddLast(viewToOpen);
			viewToOpen.PageStack = this;
			viewToOpen.Open();
		}

		public void CloseLastView()
		{
			// Закрываем последнюю вью
			var viewToClose = _viewStack.Last();
			_viewStack.RemoveLast();
			viewToClose.Close();
			// Показываем новую последнюю вью
			var viewToShow = _viewStack.LastOrDefault();
			viewToShow?.MoveIn();
		}

		public IEnumerator<BasePageView> GetEnumerator()
		{
			return _viewStack.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
