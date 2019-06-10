using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Binding
{
    //BindingContainer -> BindingNode(s) -> BindingNode(s) -> BindingElement
    //BindingContainer -> BindingElement
    //BindingNode跟BindingElement不會重複被管理
    //BindingContainer一定在UI最上層

    public static class BindingGenerator
    {
        private static GameObject getAndCheckSelectedOneGameObject()
        {
            return getAndCheckSelectedOneGameObject(Selection.objects);
        }

        private static GameObject getAndCheckSelectedOneGameObject(Object[] selectionObjects)
        {
            if (1 < selectionObjects.Length)
            {
                Debug.LogWarning("Please select only one GameObject.");
                return null;
            }

            return selectionObjects[0] as GameObject;
        }

        private static T addBinding<T>(GameObject gameObject) where T : BindingComponent
        {
            if (null == gameObject)
            {
                return null;
            }

            T t = gameObject.AddComponent<T>();

            Debug.Log("Add " + typeof(T) + " to " + gameObject.name);

            return t;
        }

        [MenuItem("GameObject/Binding/BindingElement", false, 0)]
        public static void generateBindingElement()
        {
            GameObject selectedGameObject = getAndCheckSelectedOneGameObject();
            if (null == selectedGameObject)
            {
                return;
            }

            addBinding<BindingElement>(selectedGameObject);

            updateTheTopBindingHub(selectedGameObject);
        }

        private static T getOrAddBinding<T>(GameObject gameObject) where T : BindingComponent
        {
            if (null == gameObject)
            {
                return null;
            }

            T t = gameObject.GetComponent<T>();
            if (null == t)
            {
                t = addBinding<T>(gameObject);
            }

            return t;
        }

        [MenuItem("GameObject/Binding/BindingNode", false, 1)]
        public static void GenerateBindingNode()
        {
            GameObject selectedGameObject = getAndCheckSelectedOneGameObject();
            if (null == selectedGameObject)
            {
                return;
            }

            getOrAddBinding<BindingNode>(selectedGameObject);

            updateTheTopBindingHub(selectedGameObject);
        }

        [MenuItem("GameObject/Binding/BindingContainer", false, 2)]
        public static void generateBindingContainer()
        {
            GameObject selectionGameObject = getAndCheckSelectedOneGameObject(Selection.objects);
            if (null == selectionGameObject)
            {
                return;
            }

            BindingContainer bindingContainer = getOrAddBinding<BindingContainer>(selectionGameObject);

            BindingContainer[] bindingContainerChildrens = selectionGameObject.GetComponentsInChildren<BindingContainer>(true);
            if (1 < bindingContainerChildrens.Length)
            {
                Debug.LogError("不能有同一個以上BindingContainer 在" + selectionGameObject.name + "與所有子物件");
                return;
            }

            BindingContainer[] bindingContainerParents = selectionGameObject.GetComponentsInParent<BindingContainer>(true);
            if (1 < bindingContainerParents.Length)
            {
                Debug.LogError("不能有同一個以上BindingContainer 在" + selectionGameObject.name + "與所有父物件");
                return;
            }

            updateBindingHub(bindingContainer);
        }

        /// <summary>
        /// 更新該物件最上層的BindingHub
        /// </summary>
        /// <param name="gameObject"></param>
        private static void updateTheTopBindingHub(GameObject gameObject)
        {
            if (null == gameObject)
            {
                return;
            }

            BindingHub[] bindingHubs = gameObject.GetComponentsInParent<BindingHub>(true);
            for (int i = 0; i < bindingHubs.Length; ++i)
            {
                BindingHub bindingHub = bindingHubs[i];
                BindingHub[] bindingHubs_ = bindingHub.cachedGameObject.GetComponentsInParent<BindingHub>(true);
                // 只有一個的就是最上層的了
                if (1 == bindingHubs_.Length)
                {
                    updateBindingHub(bindingHubs_[0]);
                    break;
                }
            }
        }

        private static void updateBindingHub(BindingHub bindingHub)
        {
            if (null == bindingHub)
            {
                return;
            }

            GameObject gameObject = bindingHub.cachedGameObject;

            BindingHub[] bindingHubs = gameObject.GetComponentsInChildren<BindingHub>(true);
            if (0 < bindingHubs.Length)
            {
                for (int i = 0; i < bindingHubs.Length; ++i)
                {
                    bindingHubs[i].clearBindings();
                }
            }

            bool repeated = false;

            // 找到管理BindingElement的上層
            BindingElement[] bindingElements = gameObject.GetComponentsInChildren<BindingElement>(true);
            if (0 < bindingElements.Length)
            {
                for (int i = 0; i < bindingElements.Length; ++i)
                {
                    BindingElement bindingElement = bindingElements[i];
                    BindingHub bindingHub_ = bindingElement.cachedGameObject.GetComponentInParent<BindingHub>(true);
                
                    if (null == bindingHub_)
                    {
                        continue;
                    }

                    bindingHub_.AddBinding(bindingElement.getData());

                    List<BindingData> repeatedbindingDatas = checkRepeatedBindings(bindingHub_.getBindings());
                    if (null != repeatedbindingDatas && 0 < repeatedbindingDatas.Count)
                    {
                        repeated = true;

                        for (int k = 0; k < repeatedbindingDatas.Count; ++k)
                        {
                            BindingData repeatedbindingData = repeatedbindingDatas[k];
                            Debug.LogError("Repeated bindings in : " + bindingHub_.cachedGameObject.name + ", Identifier:" + repeatedbindingData.getIdentifier() + ", Type: " + repeatedbindingData.getObject().GetType());
                        }
                    }
                }
            }

            if (repeated)
            {
                return;
            }

            // 找到管理BindingHub的上層
            if (0 < bindingHubs.Length)
            {
                for (int i = 0; i < bindingHubs.Length; ++i)
                {
                    BindingHub bindingHub_ = bindingHubs[i];

                    Transform parent = bindingHub_.cachedTransform.parent;
                    if (null == parent)
                    {
                        continue;
                    }

                    BindingHub bindingHubParent = parent.gameObject.GetComponentInParent<BindingHub>(true);
                    if (null == bindingHubParent)
                    {
                        continue;
                    }

                    BindingNode bindingNode = bindingHub_ as BindingNode;
                    if (null == bindingNode)
                    {
                        continue;
                    }

                    BindingData bindingData = new BindingData();
                    bindingData.setIdentifier(bindingNode.getIdentifier());
                    bindingData.setObject(bindingNode);
                    bindingHubParent.AddBinding(bindingData);

                    List<BindingData> bindingDatas = bindingHubParent.getBindings();

                    List<BindingData> repeatedBindingDatas = checkRepeatedBindings(bindingDatas);
                    if (null != repeatedBindingDatas && 0 < repeatedBindingDatas.Count)
                    {
                        repeated = true;

                        for (int k = 0; k < repeatedBindingDatas.Count; ++k)
                        {
                            BindingData repeatedBindingData = repeatedBindingDatas[k];
                            Debug.LogError("Repeated bindings in : " + bindingHub_.cachedGameObject.name + ", Identifier:" + repeatedBindingData.getIdentifier() + ", Type: " + repeatedBindingData.getObject().GetType());
                        }
                    }
                }
            }

            if (repeated)
            {
                return;
            }

            Debug.Log(string.Format("Do {0} Binding Done.", gameObject.name));
        }

        public static List<BindingData> checkRepeatedBindings(List<BindingData> bindings)
        {
            if (null == bindings)
            {
                return null;
            }

            List<BindingData> repeatedBinding = null;

            //Dictionary<string, Type> identifierHash = new Dictionary<string, Type>();
            Dictionary<Identifier, Type> identifierHash = new Dictionary<Identifier, Type>();
            for (int i = 0; i < bindings.Count; ++i)
            {
                BindingData binding = bindings[i];

                // 不需要Binding null的東西
                if (null == binding.getObject())
                {
                    continue;
                }

                Type type = null;
                if (identifierHash.TryGetValue(binding.getIdentifier(), out type))
                {
                    if (null != type)
                    {
                        Type bindingObjectType = binding.getObject().GetType();
                        if (type == bindingObjectType)
                        {
                            if (null == repeatedBinding)
                            {
                                repeatedBinding = new List<BindingData>();
                            }
                            repeatedBinding.Add(binding);
                        }
                    }
                }
                else
                {
                    identifierHash.Add(binding.getIdentifier(), binding.getObject().GetType());
                }
            }

            return repeatedBinding;
        }
    }
}
