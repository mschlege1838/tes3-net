
using System;

using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.INGR.IRDT 
{
	
	[HandlesSubRecord("INGR", "IRDT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var pos = 0;

            var weight = BitConverter.ToSingle(context.buf, pos); pos += 4;
            var value = BitConverter.ToInt32(context.buf, pos); pos += 4;


            var effectIds = new int[4];
            for (var i = 0; i < Constants.INGR_EFFECTS_LENGTH; ++i)
            {
                effectIds[i] = BitConverter.ToInt32(context.buf, pos); pos += 4;
            }

            var skillIds = new int[4];
            for (var i = 0; i < Constants.INGR_EFFECTS_LENGTH; ++i)
            {
                skillIds[i] = BitConverter.ToInt32(context.buf, pos); pos += 4;
            }

            var attributeIds = new int[4];
            for (var i = 0; i < Constants.INGR_EFFECTS_LENGTH; ++i)
            {
                attributeIds[i] = BitConverter.ToInt32(context.buf, pos); pos += 4;
            }

            return new IngredientData(context.SubRecordName, weight, value, effectIds, skillIds, attributeIds);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return 56;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var ingredientData = (IngredientData) context.subRecord;

            WriteBytes(context.stream, BitConverter.GetBytes(ingredientData.Weight));
            WriteBytes(context.stream, BitConverter.GetBytes(ingredientData.Value));

            for (var i = 0; i < Constants.INGR_EFFECTS_LENGTH; ++i)
            {
                WriteBytes(context.stream, BitConverter.GetBytes(ingredientData.EffectIds[i]));
            }
            for (var i = 0; i < Constants.INGR_EFFECTS_LENGTH; ++i)
            {
                WriteBytes(context.stream, BitConverter.GetBytes(ingredientData.SkillIds[i]));
            }
            for (var i = 0; i < Constants.INGR_EFFECTS_LENGTH; ++i)
            {
                WriteBytes(context.stream, BitConverter.GetBytes(ingredientData.AttributeIds[i]));
            }
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "IRDT", Names.SubRecordOrder);
		}
		
	}
	
}
