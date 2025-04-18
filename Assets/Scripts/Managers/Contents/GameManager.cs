using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    GameObject _player;

    public GameObject GetPlayer() { return _player; }

    HashSet<GameObject> _monsters = new HashSet<GameObject>();

    public Action<int> OnSpawnEvent;

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject gameObject = Managers.Resource.Instantiate(path, parent);

        switch (type)
        {
            case Define.WorldObject.Player:
                _player = gameObject;
                break;
            case Define.WorldObject.Monster:
                _monsters.Add(gameObject);
                if (OnSpawnEvent != null)
                    OnSpawnEvent.Invoke(1);
                break;
        }

        return gameObject;
    }

    public void Despawn(GameObject gameObject)
    {
        Define.WorldObject type = GetWorldObjectType(gameObject);

        switch (type)
        {
            case Define.WorldObject.Player:
                if (_player == gameObject)
                    _player = null;
                break;
            case Define.WorldObject.Monster:
                if (_monsters.Contains(gameObject))
                {
                    _monsters.Remove(gameObject);
                    if (OnSpawnEvent != null)
                        OnSpawnEvent.Invoke(-1);
                }
                break;
        }

        Managers.Resource.Destroy(gameObject);
    }

    public Define.WorldObject GetWorldObjectType(GameObject gameObject)
    {
        BaseController bc = gameObject.GetComponent<BaseController>();
        if (bc == null)
            return Define.WorldObject.Unknown;

        return bc.WorldObjectType;
    }

}
