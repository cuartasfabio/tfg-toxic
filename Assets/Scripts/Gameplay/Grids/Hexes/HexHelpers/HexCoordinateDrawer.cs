namespace Gameplay.Grids.Hexes.HexHelpers
{
    /*[CustomPropertyDrawer(typeof(HexCoordinates))]
    public class HexCoordinateDrawer: PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            HexCoordinates coords = new HexCoordinates(
                property.FindPropertyRelative("x").intValue,
                property.FindPropertyRelative("z").intValue
            );

            position = EditorGUI.PrefixLabel(position, label);
            GUI.Label(position, coords.ToString());
        }
    }*/
}