/*
*	Copyright (c) RainyRizzle Inc. All rights reserved
*	Contact to : www.rainyrizzle.com , contactrainyrizzle@gmail.com
*
*	This file is part of [AnyPortrait].
*
*	AnyPortrait can not be copied and/or distributed without
*	the express permission of [Seungjik Lee] of [RainyRizzle team].
*
*	It is illegal to download files from other than the Unity Asset Store and RainyRizzle homepage.
*	In that case, the act could be subject to legal sanctions.
*/

using UnityEngine;
//using UnityEngine.Profiling;
using System.Collections;
using System.Collections.Generic;
using System;


using AnyPortrait;

namespace AnyPortrait
{
	/// <summary>
	/// Vertex를 업데이트하는 함수가 담겨있는 객체이다.
	/// Vertex 처리에 대해서 기존의 Modifier -> CalculateStack -> Mesh -> Vertex Update 방식의 문제를 해결하기 위해
	/// 각 처리 단계에서 중요 데이터와 레이어, Weight를 for문 없이 담아두고
	/// 단 한번의 루프에서 처리할 수 있도록 한다.
	/// Calculated 연결시 미리 다 연결해둔다.
	/// </summary>
	public class apOptVertexRequest
	{
		// Members
		//-----------------------------------------------

		//Local Morph의 경우
		public class ModWeightPair
		{
			//public bool _isCalculated = false;//TODO < 이거 삭제해야 한다. (v1.4.7의 최적화 방침에 의해)
			public apOptModifiedMesh _modMesh = null;
			public float _weight = 0.0f;

			//추가 19.5.24 : ModMeshSet의 Vertex를 이용하는 경우
			public apOptModifiedMesh_Vertex _modMeshSet_Vertex = null;

			public ModWeightPair(apOptModifiedMesh modMesh)
			{
				//_isCalculated = false;//v1.4.7 : 삭제됨
				_modMesh = modMesh;
				_modMeshSet_Vertex = null;
				_weight = 0.0f;
			}

			//추가 : ModMeshSet을 이용한다.
			public ModWeightPair(apOptModifiedMesh_Vertex modMeshSet_Vertex)
			{
				//_isCalculated = false;//v1.4.7 : 삭제됨
				_modMesh = null;
				_modMeshSet_Vertex = modMeshSet_Vertex;
				_weight = 0.0f;
			}

			//v.1.4.7 삭제
			//public void InitCalculate()
			//{
			//	//_isCalculated = false;//v1.4.7 : 삭제됨
			//	_weight = 0.0f;
			//}

			//v.1.4.7 삭제
			//public void SetWeight(float weight)
			//{
			//	//_isCalculated = true;//v1.4.7 : 삭제됨
			//	_weight = weight;
			//}
		}

		//v1.4.7 : Private으로 변경 (계산된 결과 Pair와 구분하기 위해)
		private List<ModWeightPair> _modWeightPairs = new List<ModWeightPair>();
		//private int _nModWeightPairs = 0;//삭제 v1.4.7

		//계산된 ModWeightPair
		private List<ModWeightPair> _calculatedModWeightPairs = new List<ModWeightPair>();
		private int _nCalculatedModWeightPairs = 0;


		//Rigging의 경우
		//Bone + Weight는 크게 바뀌는 경우가 없다..
		//Transform, Bone만 연결해두고 Weight를 저장한 뒤 처리한다.
		//..Rigging은 첫 연결 후 weight가 변할리는 없으니 Modifier 처리가 필요 없을지도..
		public class RigBoneWeightPair
		{
			//삭제 20.11.27 : LUT 로직으로 대체되면서 사용하지 않게됨
			//public apOptTransform _optTransform = null;
			//public apMatrix3x3 _boneMatrix = apMatrix3x3.identity;

			public apOptBone _bone = null;

			//추가 20.11.26 : 빠른 계산을 위해 "미리 계산된 LUT"의 인덱스
			public int _iRigPairLUT = -1;

			public float _weight = 0.0f;

			public RigBoneWeightPair(/*apOptTransform optTransform, */apOptBone bone, float weight)
			{
				//_optTransform = optTransform;
				_bone = bone;
				_weight = weight;
				_iRigPairLUT = -1;
			}

			//삭제 20.11.27 : LUT 로직으로 대체되었다.
			//public void CalculateMatrix()
			//{
			//	_boneMatrix.SetMatrix(_optTransform._vertMeshWorldNoModInverseMatrix);
			//	_boneMatrix.Multiply(_bone._vertWorld2BoneModWorldMatrix);
			//	_boneMatrix.Multiply(_optTransform._vertMeshWorldNoModMatrix);
			//}
		}

		//버텍스별로 WeightPair를 가진다. 빠른 처리를 위해 배열로 저장
		public class VertRigWeightTable
		{
			public RigBoneWeightPair[] _rigTable = null;
			public int _nRigTable = 0;
			public float _totalRiggingWeight = 0.0f;

			public VertRigWeightTable(int vertIndex, apOptModifiedMesh modMesh)
			{
				apOptModifiedVertexRig.OptWeightPair[] weightPairs = modMesh._vertRigs[vertIndex]._weightPairs;

				_rigTable = new RigBoneWeightPair[weightPairs.Length];

				//float totalWeight = 0.0f;
				_totalRiggingWeight = 0.0f;
				for (int i = 0; i < weightPairs.Length; i++)
				{
					_rigTable[i] = new RigBoneWeightPair(/*modMesh._targetTransform, */weightPairs[i]._bone, weightPairs[i]._weight);
					_totalRiggingWeight += weightPairs[i]._weight;
				}
				//Debug.Log("VertRigWeightTable : " + totalWeight);

				//RiggingMatrix를 위해서는 무조건 Normalize를 해야한다.
				//주의 : _totalRiggingWeight이 값은 1이 아닌 원래의 Weight 합을 유지해야한다. 보간시 필요
				if (_totalRiggingWeight > 0.0f)
				{
					for (int i = 0; i < _rigTable.Length; i++)
					{
						_rigTable[i]._weight /= _totalRiggingWeight;
					}
				}
				_nRigTable = _rigTable.Length;

			}

			//추가 : ModMeshSet으로 RigWeightTable을 만드는 기능도 추가
			public VertRigWeightTable(int vertIndex, apOptModifiedMeshSet modMeshSet, apOptModifiedMesh_VertexRig modMeshSet_Rigging)
			{
				apOptModifiedMesh_VertexRig.VertexRig.WeightPair[] weightPairs = modMeshSet_Rigging._vertRigs[vertIndex]._weightPairs;

				_rigTable = new RigBoneWeightPair[weightPairs.Length];

				//float totalWeight = 0.0f;
				_totalRiggingWeight = 0.0f;
				for (int i = 0; i < weightPairs.Length; i++)
				{
					_rigTable[i] = new RigBoneWeightPair(/*modMeshSet._targetTransform, */weightPairs[i]._bone, weightPairs[i]._weight);
					_totalRiggingWeight += weightPairs[i]._weight;
				}
				//Debug.Log("VertRigWeightTable : " + totalWeight);

				//RiggingMatrix를 위해서는 무조건 Normalize를 해야한다.
				//주의 : _totalRiggingWeight이 값은 1이 아닌 원래의 Weight 합을 유지해야한다. 보간시 필요
				if (_totalRiggingWeight > 0.0f)
				{
					for (int i = 0; i < _rigTable.Length; i++)
					{
						_rigTable[i]._weight /= _totalRiggingWeight;
					}
				}
				_nRigTable = _rigTable.Length;

			}
		}

		//각 Vertex마다 Rig 정보를 넣자
		public VertRigWeightTable[] _rigBoneWeightTables = null;

		public float _totalWeight = 1.0f;
		//public float _totalRiggingWeight = 0.0f;//<<8.2 추가
		public bool _isCalculated = false;
		//public string _strDebug = "";

		public enum REQUEST_TYPE
		{
			VertLocal,
			Rigging
		}

		private REQUEST_TYPE _requestType = REQUEST_TYPE.VertLocal;

		// Init
		//-----------------------------------------------
		public apOptVertexRequest(REQUEST_TYPE requestType)
		{
			_requestType = requestType;
			Clear();
		}

		public void Clear()
		{
			if(_modWeightPairs == null)
			{
				_modWeightPairs = new List<ModWeightPair>();
			}
			_modWeightPairs.Clear();
			//_nModWeightPairs = 0;

			if(_calculatedModWeightPairs == null)
			{
				_calculatedModWeightPairs = new List<ModWeightPair>();
			}
			_nCalculatedModWeightPairs = 0;

			_rigBoneWeightTables = null;

			_totalWeight = 1.0f;
			_isCalculated = false;
		}

		// Functions
		//-----------------------------------------------
		/// <summary>
		/// v1.4.7 변경 : 최적화를 위해 ParamKeyValue에 생성된 ModWeightPair를 연결해야한다.
		/// (Rigging은 Static 타입이어서 연결할 필요 없음)
		/// </summary>
		public ModWeightPair AddModMesh(apOptModifiedMesh modMesh)
		{
			if (_requestType == REQUEST_TYPE.VertLocal)
			{
				ModWeightPair modWeightPair = new ModWeightPair(modMesh);
				_modWeightPairs.Add(modWeightPair);
				//_nModWeightPairs = _modWeightPairs.Count;

				return modWeightPair;
			}
			else if(_requestType == REQUEST_TYPE.Rigging)
			{	
				if(_rigBoneWeightTables != null)
				{
					//??
					//Rigging은 Static 타입이어서 ModMesh가 하나만 생성된다.
					Debug.LogError("Overwritten Mod Mesh To Rigging");
					return null;
				}
				//_totalRiggingWeight = 0.0f;
				_rigBoneWeightTables = new VertRigWeightTable[modMesh._vertRigs.Length];

				for (int i = 0; i < modMesh._vertRigs.Length; i++)
				{
					_rigBoneWeightTables[i] = new VertRigWeightTable(i, modMesh);
					//_totalRiggingWeight += _rigBoneWeightTables[i]._totalRiggingWeight;//<<추가 RiggingWeight를 계산합시다.
				}
			}
			return null;
		}


		/// <summary>
		/// v1.4.7 변경 : 최적화를 위해 ParamKeyValue에 생성된 ModWeightPair를 연결해야한다.
		/// (Rigging은 Static 타입이어서 연결할 필요 없음)
		/// </summary>
		public ModWeightPair AddModMeshSet(apOptModifiedMeshSet modMeshSet)
		{
			if (_requestType == REQUEST_TYPE.VertLocal)
			{	
				ModWeightPair modWeightPair = new ModWeightPair(modMeshSet.SubModMesh_Vertex);
				_modWeightPairs.Add(modWeightPair);
				//_nModWeightPairs = _modWeightPairs.Count;

				return modWeightPair;
			}
			else if(_requestType == REQUEST_TYPE.Rigging)
			{
				apOptModifiedMesh_VertexRig modMesh_Rigging = modMeshSet.SubModMesh_Rigging;
				if(_rigBoneWeightTables != null)
				{
					//??
					//Rigging은 Static 타입이어서 ModMesh가 하나만 생성된다.
					Debug.LogError("Overwritten Mod Mesh To Rigging");
					return null;
				}

				//_totalRiggingWeight = 0.0f;
				_rigBoneWeightTables = new VertRigWeightTable[modMesh_Rigging._vertRigs.Length];

				for (int i = 0; i < modMesh_Rigging._vertRigs.Length; i++)
				{
					_rigBoneWeightTables[i] = new VertRigWeightTable(i, modMeshSet, modMesh_Rigging);
				}
			}

			return null;
		}



		public void InitCalculate()
		{
			//v1.4.7 : _isCalculated 삭제를 하는 최적화 전략에 의해 다음의 코드도 변경된다.
			//이전
			//if (_requestType == REQUEST_TYPE.VertLocal)
			//{
			//	for (int i = 0; i < _nModWeightPairs; i++)
			//	{
			//		_modWeightPairs[i].InitCalculate();
			//	}
			//}

			//변경. 일일이 _isCalculated를 false로 하는 것 보다, "계산 결과 리스트"를 초기화한다.
			_calculatedModWeightPairs.Clear();
			_nCalculatedModWeightPairs = 0;


			_totalWeight = 1.0f;
			_isCalculated = false;
			//_strDebug ="<None>";
		}



		public void SetCalculated()
		{
			_isCalculated = true;
		}


		public void MultiplyWeight(float weight)
		{
			_totalWeight *= weight;
		}


		//추가 v1.4.7
		//계산된 PKV의 ModWeightPair를 직접 수정하지 않고 여기서 한다.
		public void OnModWeightPairCalculated(ModWeightPair targetModWeightPair, float weight)
		{
			if(targetModWeightPair == null)
			{
				return;
			}

			//Weight를 지정하고
			targetModWeightPair._weight = weight;

			//리스트에 넣는다. (충돌 체크는 안한다. PKV가 고유하게 동작하므로)
			_calculatedModWeightPairs.Add(targetModWeightPair);
			_nCalculatedModWeightPairs += 1;
		}
		// Get / Set
		//-----------------------------------------------
		/// <summary>
		/// v1.4.7 : 계산된 ModWeightPair들. 기존의 _isCalculated에 해당한다.
		/// </summary>
		public List<ModWeightPair> CalculatedModWeightPairs
		{
			get
			{
				return _calculatedModWeightPairs;
			}
		}

		/// <summary>
		/// v1.4.7 : 계산된 ModWeightPair들의 개수. 기존의 _isCalculated의 개수에 해당한다.
		/// </summary>
		public int NumCalculatedModWeightPairs
		{
			get { return _nCalculatedModWeightPairs; }
		}
	}
}
