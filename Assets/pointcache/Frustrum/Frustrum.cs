namespace pointcache.Frustrum {

    using UnityEngine;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Object representing Frustrum mesh
    /// 
    /// Usage : 
    ///     Call the constructor with desired parameters.
    ///     Call SetParameters, SetGenerationOptions to change parameters.
    ///         Call GenerateFull() to apply parameters.
    ///         
    /// 
    /// </summary>
    [System.Serializable]
    public class Frustrum {

        private Mesh m_mesh;

        private float m_Vfov;
        private float m_Hfov;
        private float m_nearPlane;
        private float m_farPlane;

        private bool m_genNearPlane = true;
        private bool m_genFarPlane = true;
        private bool m_genSides = true;

        private List<Vector3> m_vertices = new List<Vector3>();
        private List<int> m_triangles = new List<int>();

        public Mesh FrustrumMesh { get { return m_mesh; } }

        public Frustrum(float vertFov, float horFov, float nearPlane, float farPlane, bool splitVerts) {

            m_mesh = new Mesh();
            m_mesh.MarkDynamic();

            SetParameters(vertFov, horFov, nearPlane, farPlane);

            GenerateFull(splitVerts);
        }

        public void SetParameters(float vertFov, float horFov, float nearPlaneDist, float farPlaneDist) {

            m_Vfov = Mathf.Clamp(vertFov, 0f, 180f);
            m_Hfov = Mathf.Clamp(horFov, 0f, 180f);
            m_nearPlane = Mathf.Clamp(nearPlaneDist, 0f, float.MaxValue);
            m_farPlane = Mathf.Clamp(farPlaneDist, 0f, float.MaxValue);

        }

        /// <summary>
        /// Which planes to generate
        /// </summary>
        /// <param name="nearPlane">Generate near plane</param>
        /// <param name="farPlane">Generate far plane</param>
        /// <param name="sides">Generate sides</param>
        public void SetGenerationOptions(bool nearPlane, bool farPlane, bool sides) {
            m_genNearPlane = nearPlane;
            m_genFarPlane = farPlane;
            m_genSides = sides;
        }

        /// <summary>
        /// Generates the mesh with current parameters.
        /// </summary>
        /// <param name="splitVertices"> Split vertices if you want a pretty mesh, and disable if you need a watertight mesh, for collider for example. </param>
        public void GenerateFull(bool splitVertices) {
            Generate(splitVertices, Vector2.zero, Vector2.one);
        }

        /// <summary>
        /// Generates partial mesh limited by 2 offsets.
        /// </summary>
        /// <param name="splitVertices">Split vertices if you want a pretty mesh, and disable if you need a watertight mesh, for collider for example.</param>
        /// <param name="minOffset">Extent, removes area relative to lower left corner, valid value is in the 0-1 range</param>
        /// <param name="maxOffset">Extent, adds area relative to lower left corne, valid value is in the 0-1 ranger</param>
        public void GeneratePartial(bool splitVertices, Vector2 minExtent, Vector2 maxExtent) {
            Generate(splitVertices, minExtent, maxExtent);
        }

        //scheme
        //http://i.imgur.com/Zq7CG1a.png
        //offsets visualized
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="splitVertices">Split vertices if you want a pretty mesh, and disable if you need a watertight mesh, for collider for example.</param>
        /// <param name="minOffset">Extent, removes area relative to lower left corner, valid value is in the 0-1 range</param>
        /// <param name="maxOffset">Extent, adds area relative to lower left corner, valid value is in the 0-1 range</param>
        private void Generate(bool splitVertices, Vector2 minExtent, Vector2 maxExtent) {

            //1. First a bit of trig to find the correct witdth and height of the frustrum planes.
            float Vtan = Mathf.Tan((m_Vfov / 2f) * Mathf.Deg2Rad);
            float Htan = Mathf.Tan((m_Hfov / 2f) * Mathf.Deg2Rad);

            float nearHalfWidth = (Htan * m_nearPlane);
            float nearHalfHeight = (Vtan * m_nearPlane);
            float farHalfWidth = (Htan * m_farPlane);
            float farHalfHeight = (Vtan * m_farPlane);

            //2. Process received extents to be usable by vertices
            float near_ex_min_X = minExtent.x * (nearHalfWidth * 2f);
            float near_ex_min_Y = minExtent.y * (nearHalfHeight * 2f);
            float near_ex_max_X = (nearHalfWidth * 2f) - (maxExtent.x * (nearHalfWidth * 2f));
            float near_ex_max_Y = (nearHalfHeight * 2f) - (maxExtent.y * (nearHalfHeight * 2f));

            float far_ex_min_X = minExtent.x * (farHalfWidth * 2f);
            float far_ex_min_Y = minExtent.y * (farHalfHeight * 2f);
            float far_ex_max_X = (farHalfWidth * 2f) - (maxExtent.x * (farHalfWidth * 2f));
            float far_ex_max_Y = (farHalfHeight * 2f) - (maxExtent.y * (farHalfHeight * 2f));

            //3. Prepare frustrum planes defining points

            //0
            Vector3 A1 = new Vector3(-nearHalfWidth + near_ex_min_X, -nearHalfHeight + near_ex_min_Y, m_nearPlane);
            //1
            Vector3 A2 = new Vector3(-nearHalfWidth + near_ex_min_X, nearHalfHeight - near_ex_max_Y, m_nearPlane);
            //2
            Vector3 A3 = new Vector3(nearHalfWidth - near_ex_max_X, nearHalfHeight - near_ex_max_Y, m_nearPlane);
            //3
            Vector3 A4 = new Vector3(nearHalfWidth - near_ex_max_X, -nearHalfHeight + near_ex_min_Y, m_nearPlane);
            //4
            Vector3 B1 = new Vector3(-farHalfWidth + far_ex_min_X, -farHalfHeight + far_ex_min_Y, m_farPlane);
            //5
            Vector3 B2 = new Vector3(-farHalfWidth + far_ex_min_X, farHalfHeight - far_ex_max_Y, m_farPlane);
            //6
            Vector3 B3 = new Vector3(farHalfWidth - far_ex_max_X, farHalfHeight - far_ex_max_Y, m_farPlane);
            //7
            Vector3 B4 = new Vector3(farHalfWidth - far_ex_max_X, -farHalfHeight + far_ex_min_Y, m_farPlane);

            m_vertices.Clear();
            m_triangles.Clear();

            //4. Generate actual mesh

            // Watertight mesh
            if (!splitVertices) {

                m_vertices.Add(A1);
                m_vertices.Add(A2);
                m_vertices.Add(A3);
                m_vertices.Add(A4);
                m_vertices.Add(B1);
                m_vertices.Add(B2);
                m_vertices.Add(B3);
                m_vertices.Add(B4);


                if (m_genNearPlane) {

                    //A1,A2,A3
                    m_triangles.Add(0);
                    m_triangles.Add(1);
                    m_triangles.Add(2);

                    //A1,A4,A3
                    m_triangles.Add(2);
                    m_triangles.Add(3);
                    m_triangles.Add(0);

                }

                if (m_genFarPlane) {

                    //B4,B3,B2
                    m_triangles.Add(7);
                    m_triangles.Add(6);
                    m_triangles.Add(5);

                    //B2,B1,B4
                    m_triangles.Add(5);
                    m_triangles.Add(4);
                    m_triangles.Add(7);

                }

                if (m_genSides) {

                    // ======================= SIDE 1

                    //A2,B2,B3
                    m_triangles.Add(1);
                    m_triangles.Add(5);
                    m_triangles.Add(6);

                    //B3,A3,A2
                    m_triangles.Add(6);
                    m_triangles.Add(2);
                    m_triangles.Add(1);

                    // ======================= SIDE 2

                    //B3,B4,A4
                    m_triangles.Add(6);
                    m_triangles.Add(7);
                    m_triangles.Add(3);

                    //A4,A3,B3
                    m_triangles.Add(3);
                    m_triangles.Add(2);
                    m_triangles.Add(6);

                    // ======================= SIDE 3

                    //B4,B1,A1
                    m_triangles.Add(7);
                    m_triangles.Add(4);
                    m_triangles.Add(0);

                    //A1,A4,B4
                    m_triangles.Add(0);
                    m_triangles.Add(3);
                    m_triangles.Add(7);

                    // ======================= SIDE 4

                    //A1,B1,B2
                    m_triangles.Add(0);
                    m_triangles.Add(4);
                    m_triangles.Add(5);

                    //B2,A2,A1
                    m_triangles.Add(5);
                    m_triangles.Add(1);
                    m_triangles.Add(0);
                }
            }
            else {
                if (m_genNearPlane) {

                    //0-3
                    m_vertices.Add(A1);
                    m_vertices.Add(A2);
                    m_vertices.Add(A3);
                    m_vertices.Add(A4);

                    //A1,A2,A3
                    m_triangles.Add(0);
                    m_triangles.Add(1);
                    m_triangles.Add(2);
                    //A1,A4,A3
                    m_triangles.Add(2);
                    m_triangles.Add(3);
                    m_triangles.Add(0);

                }

                if (m_genFarPlane) {

                    //4-7
                    m_vertices.Add(B1);
                    m_vertices.Add(B2);
                    m_vertices.Add(B3);
                    m_vertices.Add(B4);

                    //B4,B3,B2
                    m_triangles.Add(7);
                    m_triangles.Add(6);
                    m_triangles.Add(5);

                    //B2,B1,B4
                    m_triangles.Add(5);
                    m_triangles.Add(4);
                    m_triangles.Add(7);

                }

                if (m_genSides) {

                    // ======================= SIDE 1

                    //8-11
                    m_vertices.Add(A2);
                    m_vertices.Add(A3);
                    //10
                    m_vertices.Add(B2);
                    m_vertices.Add(B3);

                    //A2,B2,B3
                    m_triangles.Add(8);
                    m_triangles.Add(10);
                    m_triangles.Add(11);

                    //B3,A3,A2
                    m_triangles.Add(11);
                    m_triangles.Add(9);
                    m_triangles.Add(8);

                    // ======================= SIDE 2

                    //12-15
                    m_vertices.Add(A3);
                    m_vertices.Add(A4);
                    //14
                    m_vertices.Add(B3);
                    m_vertices.Add(B4);

                    //B3,B4,A4
                    m_triangles.Add(14);
                    m_triangles.Add(15);
                    m_triangles.Add(13);

                    //A4,A3,B3
                    m_triangles.Add(13);
                    m_triangles.Add(12);
                    m_triangles.Add(14);

                    // ======================= SIDE 3

                    //16-19
                    m_vertices.Add(A1);
                    m_vertices.Add(A4);
                    //18
                    m_vertices.Add(B1);
                    m_vertices.Add(B4);

                    //B4,B1,A1
                    m_triangles.Add(19);
                    m_triangles.Add(18);
                    m_triangles.Add(16);

                    //A1,A4,B4
                    m_triangles.Add(16);
                    m_triangles.Add(17);
                    m_triangles.Add(19);

                    // ======================= SIDE 4

                    //20-23
                    m_vertices.Add(A1);
                    m_vertices.Add(A2);
                    //22
                    m_vertices.Add(B1);
                    m_vertices.Add(B2);

                    //A1,B1,B2
                    m_triangles.Add(20);
                    m_triangles.Add(22);
                    m_triangles.Add(23);

                    //B2,A2,A1
                    m_triangles.Add(23);
                    m_triangles.Add(21);
                    m_triangles.Add(20);
                }
            }

            m_mesh.Clear();
            m_mesh.SetVertices(m_vertices);
            m_mesh.SetTriangles(m_triangles.ToArray(), 0);
            m_mesh.RecalculateBounds();
            m_mesh.RecalculateNormals();

        }

        float Remap(float value, float from1, float to1, float from2, float to2) {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}