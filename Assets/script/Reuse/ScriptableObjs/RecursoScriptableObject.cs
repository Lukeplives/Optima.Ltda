using UnityEngine;

public class RecursoScriptableObject : ScriptableObject
{
    private float _speed = 1;

    public float speed
    {
        set
        {
            this._speed = value;
        }

        get
        {
            return this._speed;
        }
    }

     private Bounds _bounds;

}
