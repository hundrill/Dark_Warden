using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekoPunch.Raindrop
{
    //The Monobehaviour used to control the drop directions and positions
    [ExecuteInEditMode]
    public class RaindropsObject : MonoBehaviour
    {
        public float _LerpTime = 1.0f; //Update time interval
        Matrix4x4 _LaggedMatrix; //Previous Matrix
        Matrix4x4 _TargetMatrix; //Target Matrix
        MeshRenderer _MR = null;
        MaterialPropertyBlock _PropertyBlock = null;
        float _TmpTime = 0.0f;
        // Start is called before the first frame update
        void Start()
        {
            //Initiallize the matrices
            _LaggedMatrix = transform.localToWorldMatrix;
            _TargetMatrix = transform.localToWorldMatrix;

            _MR = GetComponent<MeshRenderer>();
            if (_PropertyBlock == null)
                _PropertyBlock = new MaterialPropertyBlock();
        }

        private void OnEnable()
        {
            //Initiallize the matrices
            _LaggedMatrix = transform.localToWorldMatrix;
            _TargetMatrix = transform.localToWorldMatrix;

            _MR = GetComponent<MeshRenderer>();
            if (_PropertyBlock == null)
                _PropertyBlock = new MaterialPropertyBlock();
        }

        private void OnDestroy()
        {

        }

        // Update is called once per frame
        void Update()
        {
            _TmpTime += Time.deltaTime;
            //If entered the next interval, then update the target and lagged matrices
            if (_TmpTime > _LerpTime)
            {
                Vector3 position = transform.position;
                Quaternion rotation = transform.rotation;
                Vector3 scale = transform.lossyScale;

                _LaggedMatrix = _TargetMatrix;
                _TargetMatrix = Matrix4x4.TRS(position, rotation, scale);
                _TmpTime = _TmpTime - _LerpTime;
            }


            _MR.GetPropertyBlock(_PropertyBlock);
            _PropertyBlock.SetMatrix("_PrevMatrix", _LaggedMatrix);
            _PropertyBlock.SetMatrix("_TargetMatrix", _TargetMatrix);
            //Lerp drop effect at 2 time spots by current time 
            _PropertyBlock.SetFloat("_LerpWeight", Mathf.Clamp01(_TmpTime / _LerpTime));

            _MR.SetPropertyBlock(_PropertyBlock);
        }
    }
}