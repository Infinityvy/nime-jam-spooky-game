using UnityEditor;
using UnityEngine;

public class BitFieldAttribute : PropertyAttribute { }


[CustomPropertyDrawer(typeof(BitFieldAttribute))]
public class BitFieldDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.Integer)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Get the current int value and mask the upper bits to ensure we only modify the lower 4 bits
            int value = property.intValue & 0b00001111; // Only focus on the lower 4 bits

            // Reserve space for 4 toggle buttons (for the lower 4 bits)
            int toggleWidth = 20;
            Rect toggleRect = new Rect(position.x, position.y, toggleWidth, position.height);

            // Draw the 4 bits (from bit 3 to bit 0)
            for (int i = 3; i >= 0; i--)
            {
                // Check if the specific bit is set
                bool bit = (value & (1 << i)) != 0;

                // Toggle button for each bit
                bit = EditorGUI.Toggle(toggleRect, bit);

                // Update the value of the int if the bit changes
                if (bit)
                    value |= (1 << i); // Set bit
                else
                    value &= ~(1 << i); // Clear bit

                // Move to the next toggle button
                toggleRect.x += toggleWidth;
            }

            // Ensure that only the lower 4 bits are modified and the rest remain unchanged
            property.intValue = (property.intValue & ~0b00001111) | (value & 0b00001111);

            EditorGUI.EndProperty();
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use BitField with int.");
        }
    }
}
