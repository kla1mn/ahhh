using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerState))]
public class PlayerStateCustomEditor : Editor
{
    SerializedProperty type;

    private void OnEnable()
    {
        // Получаем ссылку на сериализованное свойство privateValue
        type = serializedObject.FindProperty("controllerType");
    }

    public override void OnInspectorGUI()
    {
        // Обновляем сериализованный объект
        serializedObject.Update();

        // Показываем privateValue только вне PlayMode
        if (!Application.isPlaying)
        {
            EditorGUILayout.PropertyField(type);
        }

        // Отображаем стандартный инспектор
    
        // Показываем значения только для чтения в PlayMode
        if (Application.isPlaying)
        {
            PlayerState player = (PlayerState)target;

            EditorGUILayout.LabelField("IsRight", player.IsFacingRight.ToString());
            EditorGUILayout.LabelField("Flipping", player.IsFliping.ToString());
            EditorGUILayout.LabelField("Moving", player.IsMoving.ToString());
            EditorGUILayout.LabelField("Jumping", player.IsJumping.ToString());
            EditorGUILayout.LabelField("Falling", player.IsFalling.ToString());
            EditorGUILayout.LabelField("AboveGround", player.IsAboveGround.ToString());
            EditorGUILayout.LabelField("Dashing", player.IsDashing.ToString());
            EditorGUILayout.LabelField("Sliding", player.IsSliding.ToString());
            EditorGUILayout.LabelField("GroundStunn", player.IsGroundStunning.ToString());
            EditorGUILayout.LabelField("Hearting", player.IsHearting.ToString());
            EditorGUILayout.LabelField("Dead", player.IsDead.ToString());
            EditorGUILayout.LabelField("Attacking", player.IsAttacking.ToString());
            EditorGUILayout.LabelField("ComboWaiting", player.IsComboWaiting.ToString());
            EditorGUILayout.LabelField("CurrentAttack", player.CurrentAttack.ToString());
            EditorGUILayout.LabelField("CanAttack", player.CanAttack.ToString());

            Repaint();
        }

        // Применяем изменения к сериализованному объекту
        serializedObject.ApplyModifiedProperties();
    }
}
