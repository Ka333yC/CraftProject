using Assets._Scripts.Core.BlocksCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.Core.Extensions
{
	public static class IBlockContainerExtensions
	{
		public static bool TryGetComponentContainer<T>(this IBlockContainer blockContainer,
			out T result) where T : IBlockComponentContainer
		{

			foreach(var componentContainer in blockContainer.BlockComponentContainers)
			{
				if(componentContainer is T resultContainer)
				{
					result = resultContainer;
					return true;
				}
			}

			result = default;
			return false;
		}
	}
}
