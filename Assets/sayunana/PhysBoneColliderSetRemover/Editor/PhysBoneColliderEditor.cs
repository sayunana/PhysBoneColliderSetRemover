#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace sayunana
{
    public class PhysBoneColliderEditor : EditorWindow
    {
        private Editor _editor;

        [SerializeField] private List<VRCPhysBone> _vrcPhysBoneList = new List<VRCPhysBone>();

        [SerializeField] private List<VRCPhysBoneCollider> _vrcPhysBoneColliderList = new List<VRCPhysBoneCollider>();

        [MenuItem("sayunana/PhysBoneCollider Set Remover")]
        static void Open()
        {
            var window = GetWindow<PhysBoneColliderEditor>();
            window.titleContent = new GUIContent("PhysBoneCollider Set Remover");
        }

        private void OnGUI()
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.wordWrap = true;
            GUILayout.Label("「VRCPhysBoneSelectList」に登録した「VRCPhysBone」に対して、「VRCPhysBoneColliderSelectList」に登録した「VRCPhysBoneCollider」を追加または削除をします",style);
            
            var so = new SerializedObject(this);

            so.Update();

            EditorGUILayout.PropertyField(so.FindProperty("_vrcPhysBoneList"), new GUIContent("VRCPhysBoneSelectList"), true);

            EditorGUILayout.PropertyField(so.FindProperty("_vrcPhysBoneColliderList"),
                new GUIContent("VRCPhysBoneColliderSelectList"), true);

            so.ApplyModifiedProperties();

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Add"))
                {
                    if (IsCheckList())
                    {
                        _vrcPhysBoneList.ForEach(vrcPhysBone =>
                        {
                            _vrcPhysBoneColliderList.ForEach(vrcPhysBoneCollider =>
                            {
                                if (!vrcPhysBone.colliders.Contains(vrcPhysBoneCollider))
                                {
                                    vrcPhysBone.colliders.Add(vrcPhysBoneCollider);
                                    Undo.RecordObject(vrcPhysBone, "Add VRCPhysBoneCollider");
                                    EditorUtility.SetDirty(vrcPhysBone);
                                }
                            });
                        });
                    }
                }

                if (GUILayout.Button("Remove"))
                {
                    if (IsCheckList())
                    {
                        _vrcPhysBoneList.ForEach(vrcPhysBone =>
                        {
                            _vrcPhysBoneColliderList.ForEach(vrcPhysBoneCollider =>
                            {
                                if (vrcPhysBone.colliders.Contains(vrcPhysBoneCollider))
                                {
                                    vrcPhysBone.colliders.Remove(vrcPhysBoneCollider);
                                    Undo.RecordObject(vrcPhysBone, "Remove VRCPhysBoneCollider");
                                    EditorUtility.SetDirty(vrcPhysBone);
                                }
                            });
                        });
                    }
                }
            }
        }

        private bool IsCheckList()
        {
            bool isCheck = true;
            if (_vrcPhysBoneList.Count == 0)
            {
                Debug.LogError("VRCPhysBoneSelectListに登録がありません");
                isCheck = false;
            }

            if (_vrcPhysBoneColliderList.Count == 0)
            {
                Debug.LogError("VRCPhysBoneColliderSelectListに登録がありません");
                isCheck = false;
            }
            return isCheck;
        }
    }
}
#endif