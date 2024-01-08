using EZhex1991.EZSoftBone;
using ModelReplacement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LethalCroc
{
    internal class AvatarReplacement : BodyReplacementBase
    {
        //Required universally
        protected override GameObject LoadAssetsAndReturnModel()
        {
            //Replace with the Asset Name from your unity project 
            string model_name = "LethalCrocPrefab";
            GameObject crocPrefab = Assets.MainAssetBundle.LoadAsset<GameObject>(model_name);

            EZSoftBone bone = crocPrefab.AddComponent<EZSoftBone>();
            bone.rootBones = new List<Transform>();
            Transform tailBone = crocPrefab.transform.Find("Armature.001/spine/Bone");
            bone.rootBones.Add(tailBone);
            bone.gravity = new Vector3(0, -3, 0);

            return crocPrefab;
        }
    }
}
