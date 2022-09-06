using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParticleID
{
    BUILDINGCOLLAPSE,
    BUILD,
    TV,
    UNITDESTROY,
}

public class ParticleMNG : Singleton<ParticleMNG>
{
    [SerializeField] GameObject prefBuildingCollapse = null;
    [SerializeField] GameObject prefBuild = null;
    [SerializeField] GameObject prefTVParticle = null;
    [SerializeField] GameObject prefUnitDestroy = null;

    GameObject SetGameObj(ParticleID _particleID)
    {
        switch (_particleID)
        {
            case ParticleID.BUILDINGCOLLAPSE: return prefBuildingCollapse;
            case ParticleID.BUILD: return prefBuild;
            case ParticleID.TV: return prefTVParticle;
            case ParticleID.UNITDESTROY: return prefUnitDestroy;
            default: return null;
        }
    }

    public void SpawnParticle(ParticleID _particleID, Vector3 _spawnPos)
    {
        GameObject pref = SetGameObj(_particleID);
        if (pref == null)
            return;

        GameObject particleTemp = Instantiate(pref, transform);
        particleTemp.transform.position = _spawnPos;
        particleTemp.GetComponent<ParticleSystem>().Play();
    }
}