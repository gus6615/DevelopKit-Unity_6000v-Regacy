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
using System.Collections;
using System.Collections.Generic;
using System;


using AnyPortrait;

namespace AnyPortrait
{
	[Serializable]
	public class apModifier_AnimatedColorOnly : apModifierBase
	{
		// Members
		//----------------------------------------------

		// Init
		//----------------------------------------------
		public override void SetInitSetting(int uniqueID, int layer, int meshGroupID, apMeshGroup meshGroup)
		{
			base.SetInitSetting(uniqueID, layer, meshGroupID, meshGroup);

			//색상 모디파이어는 색상 옵션이 기본으로 켜진다.
			_isColorPropertyEnabled = true;
		}

		// Get / Set
		//----------------------------------------------
		public override MODIFIER_TYPE ModifierType
		{
			get { return MODIFIER_TYPE.AnimatedColorOnly; }
		}

		public override apModifierParamSetGroup.SYNC_TARGET SyncTarget
		{
			get { return apModifierParamSetGroup.SYNC_TARGET.KeyFrame; }
		}

		private const string NAME_TF_ANIMATION_LONG = "Color Only (Animation)";
		private const string NAME_TF_ANIMATION_SHORT = "Color (Anim)";

		public override string DisplayName
		{
			//get { return "Transform (Animation)"; }
			get { return NAME_TF_ANIMATION_LONG; }
		}

		public override string DisplayNameShort
		{
			//get { return "Transform (Anim)"; }
			get { return NAME_TF_ANIMATION_SHORT; }
		}

		/// <summary>
		/// Calculate 계산시 어느 단계에서 적용되는가
		/// </summary>
		public override apCalculatedResultParam.CALCULATED_VALUE_TYPE CalculatedValueType
		{
			get
			{
				return apCalculatedResultParam.CALCULATED_VALUE_TYPE.Color;
			}
		}

		public override apCalculatedResultParam.CALCULATED_SPACE CalculatedSpace
		{
			get { return apCalculatedResultParam.CALCULATED_SPACE.Local; }
		}

		public override apModifiedMesh.MOD_VALUE_TYPE ModifiedValueType
		{
			get
			{
				return apModifiedMesh.MOD_VALUE_TYPE.Color;
			}
		}


		// MeshTransform / MeshGroupTransform / Bone 모두 적용한다.
		public override bool IsTarget_MeshTransform { get { return true; } }
		public override bool IsTarget_MeshGroupTransform { get { return true; } }
		public override bool IsTarget_Bone { get { return false; } }
		public override bool IsTarget_ChildMeshTransform { get { return true; } }//<<Child Mesh에 대해서도 TF가 가능하다

		public override bool IsAnimated { get { return true; } }

		//추가
		public override bool IsPhysics { get { return false; } }
		public override bool IsVolume { get { return false; } }

		///// <summary>
		///// ExEdit 중 GeneralEdit 모드에서 "동시에 작업 가능하도록 허용 된 Modifier Type들"을 리턴한다.
		///// </summary>
		///// <returns></returns>
		//public override MODIFIER_TYPE[] GetGeneralExEditableModTypes()
		//{
		//	return _generalExEditableModType;
		//}

		// Functions
		//----------------------------------------------
		public override void InitCalculate(float tDelta)
		{
			base.InitCalculate(tDelta);

			if (_calculatedResultParams.Count == 0)
			{
				return;
			}

			apCalculatedResultParam calParam = null;
			for (int iCalParam = 0; iCalParam < _calculatedResultParams.Count; iCalParam++)
			{
				calParam = _calculatedResultParams[iCalParam];
				calParam.InitCalculate();
				//calParam._isAvailable = false;//삭제 22.5.11
			}
		}

		public override void Calculate(float tDelta)
		{
			base.Calculate(tDelta);

			//CalculatePattern_Transform(tDelta);//TF
			CalculatePattern_ColorOnly(tDelta);
		}

		public override void Calculate_DLL(float tDelta)
		{
			base.Calculate_DLL(tDelta);

			//CalculatePattern_Transform_DLL(tDelta);//TF

			//Color는 DLL 모드가 없다.
			CalculatePattern_ColorOnly(tDelta);
		}
	}
}
