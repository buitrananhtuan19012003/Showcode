using UnityEngine;
using System.Collections;

namespace LupinrangerPatranger.UIWidgets
{
    public interface IValidation<T>
    {
        bool Validate(T item);
    }
}