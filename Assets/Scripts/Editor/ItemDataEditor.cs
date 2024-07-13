using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemData), true)]
public class ItemDataEditor : Editor
{
    private SerializedProperty _itemIdProp;

    private void OnEnable()
    {
        _itemIdProp = serializedObject.FindProperty($"<{nameof(ItemData.ItemId)}>k__BackingField");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        // ������ ���̵� ������ �빮�ڷ� ��ȯ
        string originalItemId = _itemIdProp.stringValue;
        string newItemId = originalItemId.ToUpper();
        if (!originalItemId.Equals(newItemId))
        {
            _itemIdProp.stringValue = newItemId;
            var itemData = ItemDatabase.Instance.FindItemById(newItemId);
            if (itemData != null)
            {
                Debug.LogWarning($"{newItemId} id already exist : {AssetDatabase.GetAssetPath(itemData)}");
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
