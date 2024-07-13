using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPattern.CreatePattern.FactoryMethod
{
    public class UnitCreateController : MonoBehaviour
    {
        Generate_A generate_A = null;
        Generate_B generate_B = null;

        void Start()
        {
            generate_A = new();
            generate_B = new();
        }

        public void DoGenerateA()
        {
            generate_A.CreateUnit();

            List<Unit> units = generate_A.Units;
            foreach (Unit unit in units)
            {
                // 각 유닛 처리
            }
        }

        public void DoGenerateB()
        {
            generate_B.CreateUnit();

            List<Unit> units = generate_B.Units;
            foreach (Unit unit in units)
            {
                // 각 유닛 처리
            }
        }
    }
}