using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Body : MonoBehaviour
{
    
    public GameObject headPrefab;
    public GameObject bodyPrefab;

    public Transform headBone;  // 본 계층의 머리 노드
    public Transform bodyBone; // 본 계층의 몸통 노드

    // Start is called before the first frame update
    void Start()
    {
        AttachPart(bodyPrefab, bodyBone);
        AttachPart(headPrefab, headBone);
    }

    private void AttachPart(GameObject partPrefab, Transform bone)
    {
        GameObject partInstance = Instantiate(partPrefab, bone);
        SkinnedMeshRenderer smr = partInstance.GetComponent<SkinnedMeshRenderer>();

        if (smr != null)
        {
            smr.rootBone = bone.root;          // 본 계층의 루트 본 연결
            smr.bones = bone.root.GetComponentsInChildren<Transform>(); // 본 계층 전체 적용
        }
    }
}
