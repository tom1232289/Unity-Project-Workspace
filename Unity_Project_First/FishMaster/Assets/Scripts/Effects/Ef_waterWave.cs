using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ef_waterWave : MonoBehaviour {

    // 公有引用
    public Texture[] m_textures;

    // 私有引用
    private Material m_material;
    private int index = 0;

    private void Awake() {
        m_material = GetComponent<MeshRenderer>().material;
    }

    private void Start() {
        InvokeRepeating("ChangeTexture", 0, 0.1f);
    }

    private void ChangeTexture() {
        m_material.mainTexture = m_textures[index];
        index = (index + 1) % m_textures.Length;    // index 在范围内加一，不能造成数组越界
    }
}
