using Assets.Scripts.Apart.ZenjectToEcs;
using Assets.Scripts.Core.GraphicsCore;
using Assets.Scripts.Core.InputCore;
using Assets.Scripts.Core.InventoryCore;
using Assets.Scripts.Core.PhysicsCore;
using Assets.Scripts.Core.PlayerCore;
using Assets.Scripts.Implementation.Chunk;
using ChunkCore;
using Extensions.Ecs;
using Leopotam.EcsLite;
using PhysicsCore.ObjectPhysics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using Voody.UniLeo.Lite;

namespace Assets._Scripts
{
	public class EcsGameStartup : MonoBehaviour
	{
		[SerializeField]
		private bool _enableDebugTool;

		[Inject]
		private DiContainer _container;
		[Inject]
		private EcsWorld _world;

		private EcsSystems _fixedUpdateSystems;
		private EcsSystems _standartUpdateSystems;
		private EcsSystems _lateUpdateSystems;

		private void Awake()
		{
			_fixedUpdateSystems = new EcsSystems(_world);
			AddSystemsToFixedUpdate();
			_standartUpdateSystems = new EcsSystems(_world);
			AddSystemsToStandartUpdate();
			_lateUpdateSystems = new EcsSystems(_world);
			AddSystemsToLateUpdate();
#if UNITY_EDITOR
			if(_enableDebugTool)
			{
				AddDebugTools();
			}
#endif
			_fixedUpdateSystems.ConvertScene();
		}

		private void Start()
		{
			_fixedUpdateSystems.Init();
			_standartUpdateSystems.Init();
			_lateUpdateSystems.Init();
		}

		private void FixedUpdate()
		{
			_fixedUpdateSystems.Run();
		}

		private void Update()
		{
			_standartUpdateSystems.Run();
		}

		private void LateUpdate()
		{
			_lateUpdateSystems.Run();
		}

		private void OnDestroy()
		{
			if(_fixedUpdateSystems != null)
			{
				_fixedUpdateSystems.Destroy();
				_fixedUpdateSystems = null;
			}

			if(_standartUpdateSystems != null)
			{
				_standartUpdateSystems.Destroy();
				_standartUpdateSystems = null;
			}

			if(_world != null)
			{
				_world.Destroy();
				_world = null;
			}
		}

		private void AddSystemsToFixedUpdate()
		{
			_fixedUpdateSystems.AddRange(ChunkCoreSystems.GetFixedInitCreatorSystems());
			_fixedUpdateSystems.AddRange(ObjectPhysicsCoreSystems.GetFixedInitCreatorSystems());
			_fixedUpdateSystems.AddRange(ChunkPhysicsCoreSystems.GetFixedInitCreatorSystems());
			_fixedUpdateSystems.AddRange(ChunkImplementationSystems.GetFixedInitCreatorSystems());

			_fixedUpdateSystems.AddRange(ChunkCoreSystems.GetFixedSystems());
			// _fixedUpdateSystems.AddRange(InputProcessSystems.GetFixedSystems());
			_fixedUpdateSystems.AddRange(ObjectPhysicsCoreSystems.GetFixedSystems());
			_fixedUpdateSystems.AddRange(ChunkPhysicsCoreSystems.GetFixedSystems());
			_fixedUpdateSystems.AddRange(PlayerCoreSystems.GetFixedSystems());

			_fixedUpdateSystems.AddRange(ChunkCoreSystems.GetPostFixedDelSystems());

			_container.InjectEcsSystems(_fixedUpdateSystems);
		}

		private void AddSystemsToStandartUpdate()
		{
			_standartUpdateSystems.AddRange(ChunkCoreSystems.GetStandartInitCreatorSystems());
			_standartUpdateSystems.AddRange(ChunkGraphicsCoreSystems.GetStandartInitCreatorSystems());

			_standartUpdateSystems.AddRange(ChunkCoreSystems.GetStandartSystems());
			_standartUpdateSystems.AddRange(ChunkGraphicsCoreSystems.GetStandartSystems());
			_standartUpdateSystems.AddRange(InventoryCoreSystems.GetStandartSystems());

			_standartUpdateSystems.AddRange(ChunkCoreSystems.GetPostStandartDelSystems());
			_standartUpdateSystems.AddRange(ObjectPhysicsCoreSystems.GetPostStandartDelSystems());
			_standartUpdateSystems.AddRange(InputCoreSystems.GetPostStandartDelSystems());

			_container.InjectEcsSystems(_standartUpdateSystems);
		}

		private void AddSystemsToLateUpdate()
		{
			//_lateUpdateSystems
			//	.Add(new ChangeLookHandlerSystem())
			//	.DelHere<ChangeLookDirectionComponent>();

			_container.InjectEcsSystems(_lateUpdateSystems);
		}

#if UNITY_EDITOR
		private void AddDebugTools()
		{
			_fixedUpdateSystems
				   // Регистрируем отладочные системы по контролю за состоянием мира.
				   .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
				   // Регистрируем отладочные системы по контролю за текущей группой систем. 
				   .Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem("Fixed update systems"));
			_standartUpdateSystems
				   // Регистрируем отладочные системы по контролю за текущей группой систем. 
				   .Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem("Standart update systems"));
			_lateUpdateSystems
				   // Регистрируем отладочные системы по контролю за текущей группой систем. 
				   .Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem("Late update systems"));
		}
#endif
	}
}
