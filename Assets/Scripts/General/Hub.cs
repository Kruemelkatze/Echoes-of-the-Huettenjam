using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace General
{
    /// <summary>
    /// This class is an implementation of the Service Locator (Anti-)Pattern (= Poor Man's Dependency Injection).
    /// It can be used to hold objects that should be commonly available.
    /// While Singletons can be achieved to do the same, this approach is more flexible on the long run.
    ///
    /// The dependencies are identified by type and an optional String identifier. The identifier should only be used
    /// if multiple variations for a type (e.g. Transforms for player and world origin) are being registered.
    ///
    /// Register can either be done for the specific type only (default) or for all implemented interfaces and/or base
    /// classes by setting the bindType parameter.
    /// 
    /// Usage:
    ///     Registration in Awake() using        Hub.Register(this);
    ///     Resolution in Start() using          _exampleService = Hub.Get<IExampleService>();
    ///
    /// Registration should be done in Awake(), Resolution should be done in Start() to guarantee all registration is
    /// done before any resolution.
    ///
    /// Example using Interfaces:
    ///     public class ExampleServiceImpl : MonoBehaviour, IExampleService
    ///     {
    ///         private void Awake()
    ///         {
    ///             Hub.Register(this, BindType.AllInterfacesAndSelf);
    ///         }
    ///     }
    ///
    ///     public class SomeScript : MonoBehaviour
    ///     {
    ///         private IExampleService _exampleService;
    ///
    ///         private void Start()
    ///         {
    ///             _exampleService = Hub.Get<IExampleService>();
    ///         } 
    ///     }
    /// 
    /// </summary>
    public class Hub
    {
        private static bool _validateOnSceneLoaded = true;

        private readonly Dictionary<(Type type, string id), object> _registrations =
            new Dictionary<(Type type, string id), object>();

        private static Hub _instance;

        public static Hub Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Hub();
                }

                return _instance;
            }
        }

        public static void OverrideInstance(Hub overrideHub)
        {
            Debug.LogWarning("Overriding Hub instance. I hope you are sure what you are doing.");
            _instance = overrideHub;
        }

        public Hub()
        {
            SceneManager.sceneUnloaded += ValidateRegistrationsOnSceneUnload;
        }

        ~Hub()
        {
            SceneManager.sceneUnloaded -= ValidateRegistrationsOnSceneUnload;
        }

        #region PUBLIC ACCESSORS

        public static T Get<T>(string id = null)
        {
            return Instance._registrations.TryGetValue((typeof(T), id), out var obj) ? (T) obj : default;
        }

        public static object Get(Type type, string id = null)
        {
            return Instance._registrations.TryGetValue((type, id), out var obj) ? obj : default;
        }

        public static void Register<T>(T obj, BindType bindType = BindType.Self, string id = null)
        {
            var type = typeof(T);
            Register(type, obj, bindType, id);
        }

        public static void Register(Type type, object obj, BindType bindType = BindType.Self, string id = null)
        {
            var baseTypes = bindType switch
            {
                BindType.AllInterfacesAndSelf => type.GetInterfaces(),
                BindType.AllInterfacesAndBaseClassesAndSelf => type.GetBaseTypes().Concat(type.GetInterfaces()),
                _ => null
            };

            if (baseTypes != null)
            {
                foreach (var baseType in baseTypes)
                {
                    if (baseType == typeof(MonoBehaviour) || baseType == typeof(Behaviour) ||
                        baseType == typeof(Component) || baseType == typeof(Object))
                    {
                        continue;
                    }

                    RegisterForType(obj, baseType, id);
                }
            }

            RegisterForType(obj, type, id);
        }

        public static void Unregister<T>(BindType bindType = BindType.Self, string id = null)
        {
            var type = typeof(T);

            var baseTypes = bindType switch
            {
                BindType.AllInterfacesAndSelf => type.GetInterfaces(),
                BindType.AllInterfacesAndBaseClassesAndSelf => type.GetBaseTypes().Concat(type.GetInterfaces()),
                _ => null
            };

            if (baseTypes != null)
            {
                foreach (var baseType in baseTypes)
                {
                    if (baseType == typeof(MonoBehaviour) || baseType == typeof(Object))
                    {
                        continue;
                    }

                    UnregisterForType(baseType, id);
                }
            }

            UnregisterForType(type, id);
        }

        #endregion

        #region PRIVATE REGISTRATION LOGIC

        private static void UnregisterForType(Type type, string id)
        {
            Instance._registrations.Remove((type, id));
        }

        private static void UnregisterForType<T>(string id)
        {
            Instance._registrations.Remove((typeof(T), id));
        }

        private static void RegisterForType(object obj, Type type, string id)
        {
            if (Get(type, id) != null)
            {
                Debug.LogWarning(
                    $"Duplicate Registration for type {type.Name}. If this is intended, unregister the Type before.");
            }

            Instance._registrations[(type, id)] = obj;
        }

        private void ValidateRegistrationsOnSceneUnload(Scene arg0)
        {
            if (_validateOnSceneLoaded)
            {
                ValidateRegistrations();
            }
        }

        private void ValidateRegistrations()
        {
            var keysToRemove = new List<(Type type, string id)>();
            foreach (var registration in _registrations)
            {
                if (registration.Value == null || registration.Value is Object obj && !obj)
                {
                    keysToRemove.Add(registration.Key);
                }
            }

            foreach (var keyToRemove in keysToRemove)
            {
                _registrations.Remove(keyToRemove);
            }
        }

        #endregion

        #region DEBUG

        public static string PrintRegistrations()
        {
            var str = "";
            foreach (var registration in Instance._registrations)
            {
                var keyStr =
                    $"{registration.Key.type.Name}{(registration.Key.id != null ? "|" + registration.Key.id : "")}";
                var valueStr = registration.Value != null ? $"{registration.Value}" : "null";

                var regStr = $"{keyStr}: {valueStr}";
                str += $"{regStr}\n";
                Debug.Log(regStr);
            }

            return str;
        }

        #endregion
    }

    public enum BindType
    {
        Self,
        AllInterfacesAndSelf,
        AllInterfacesAndBaseClassesAndSelf
    }
}