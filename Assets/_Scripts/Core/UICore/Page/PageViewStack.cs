using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace _Scripts.Core.UICore.Page
{
	public class PageViewStack : IEnumerable<BasePageView>
	{
		private readonly LinkedList<BasePageView> _viewStack = new LinkedList<BasePageView>();
		
		public BasePageView ActiveView => _viewStack.Last.Value;

		public void OpenView(BasePageView viewToOpen)
		{
			// �������� ��������� ���
			var viewToMoveOut = _viewStack.LastOrDefault();
			viewToMoveOut?.MoveOut();
			// ��������� ����� ��� ����� ���������
			_viewStack.AddLast(viewToOpen);
			viewToOpen.PageStack = this;
			viewToOpen.Open();
		}

		public void CloseLastView()
		{
			// ��������� ��������� ���
			var viewToClose = _viewStack.Last();
			_viewStack.RemoveLast();
			viewToClose.Close();
			// ���������� ����� ��������� ���
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
