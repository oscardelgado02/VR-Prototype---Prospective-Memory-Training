using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class PropertyDrawersHelper
{
#if UNITY_EDITOR

    public static string[] TaskList()
    {
        return labels.Instance._taskLabels;
    }

    public static string[] ObjectTagList()
    {
        return labels.Instance._objectTags;
    }

    public static string[] ContainerTagList()
    {
        return labels.Instance._containerTags;
    }

#endif
}