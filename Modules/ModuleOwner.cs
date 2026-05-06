using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

namespace YIS.Code.Modules
{
    public class ModuleOwner : MonoBehaviour
    {
        protected Dictionary<Type, IModule> moduleDict;

        protected virtual void Awake()
        {
            moduleDict = GetComponentsInChildren<IModule>()
                .ToDictionary(module => module.GetType());

            InitializeModules();
            AfterInitializeModules();
        }

        protected virtual void InitializeModules()
        {
            foreach (IModule module in moduleDict.Values)
            {
                module.Initialize(this);
            }
        }
        
        protected virtual void AfterInitializeModules()
        {
            foreach (IAfterInitModule module in moduleDict.Values.OfType<IAfterInitModule>())
            {
                module.AfterInitialize();
            }
        }
        
        public IModule GetModule(Type type)
            => moduleDict.GetValueOrDefault(type);

        public T GetModule<T>()
        {
            if (moduleDict.TryGetValue(typeof(T), out IModule module))
            {
                return (T)module;
            }

            IModule findModule = moduleDict.Values.FirstOrDefault(m => m is T);
            
            if (findModule != null && findModule is T castedModule)
                return castedModule;

            return default;
        }
        
    }
}