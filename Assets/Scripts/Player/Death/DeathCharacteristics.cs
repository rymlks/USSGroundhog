using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeathCharacteristics
{
    private Dictionary<string, object> _deathCharacteristics;

    public DeathCharacteristics(Dictionary<string, object> characteristics)
    {
        this._deathCharacteristics = characteristics;
    }

    public bool shouldProduceRagdollCorpse()
    {
        return _deathCharacteristics.Keys.Contains("ragdoll");
    }

    public bool shouldProduceElectrocutedCorpse()
    {
        return _deathCharacteristics.Keys.Contains("electrocution");
    }

    public bool shouldProduceNormalCorpse()
    {
        return !(shouldBurnCorpse() || shouldProduceRagdollCorpse() ||
                 shouldProduceRigidCorpse());
    }

    public bool shouldProduceRigidCorpse()
    {
        return _deathCharacteristics.Keys.Contains("freezing");
    }

    public bool shouldBurnCorpse()
    {
        return _deathCharacteristics.Keys.Contains("burning");
    }

    public bool shouldPropelCorpse()
    {
        return _deathCharacteristics.Keys.Contains("explosion");
    }

    public Vector3 getExplosionStrength()
    {
        return _deathCharacteristics["explosion"] != null ? (Vector3) _deathCharacteristics["explosion"] : Vector3.zero;
    }

    public bool shouldProduceCorpse()
    {
        return shouldProduceRagdollCorpse() || shouldProduceRigidCorpse() || shouldProduceNormalCorpse();
    }

}
