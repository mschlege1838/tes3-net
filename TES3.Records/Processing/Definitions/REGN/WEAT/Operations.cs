
using TES3.Records.SubRecords;
using static TES3.Records.Processing.RawUtils;

namespace TES3.Records.Processing.Definitions.REGN.WEAT 
{
	
	[HandlesSubRecord("REGN", "WEAT")]
    static class Operations
	{
		[HandlesOperation(SubRecordOperationType.Read)]
		internal static object Read(SubRecordReadContext context)
		{
        
            var clear = context.buf[0];
            var cloudy = context.buf[1];
            var foggy = context.buf[2];
            var overcast = context.buf[3];
            var rain = context.buf[4];
            var thunder = context.buf[5];
            var ash = context.buf[6];
            var blight = context.buf[7];

            if (context.header.Size == 8)
            {
                return new WeatherData(context.SubRecordName, clear, cloudy, foggy, overcast, rain, thunder, ash, blight);
            }

            var snow = context.buf[8];
            var blizzard = context.buf[9];
            return new ExpansionWeatherData(context.SubRecordName, clear, cloudy, foggy, overcast, rain, thunder, ash, blight, snow, blizzard);
		}
		
		[HandlesOperation(SubRecordOperationType.Size)]
		internal static object GetSize(SubRecordLengthContext context)
		{
            return context.subRecord is ExpansionWeatherData ? 10 : 8;
		}
		
		[HandlesOperation(SubRecordOperationType.Write)]
		internal static object Write(SubRecordWriteContext context)
		{
        
            var weatherData = (WeatherData) context.subRecord;

            context.stream.WriteByte(weatherData.Clear);
            context.stream.WriteByte(weatherData.Cloudy);
            context.stream.WriteByte(weatherData.Foggy);
            context.stream.WriteByte(weatherData.Overcast);
            context.stream.WriteByte(weatherData.Rain);
            context.stream.WriteByte(weatherData.Thunder);
            context.stream.WriteByte(weatherData.Ash);
            context.stream.WriteByte(weatherData.Blight);

            if (weatherData is ExpansionWeatherData exp)
            {
                context.stream.WriteByte(exp.Snow);
                context.stream.WriteByte(exp.Blizzard);
            }
			return null;
		}
		
		[HandlesOperation(SubRecordOperationType.AddIndex)]
		internal static object GetAddIndex(SubRecordAddIndexContext context)
		{
			return GetAddIndexOrdered(context.record, "WEAT", Names.SubRecordOrder);
		}
		
	}
	
}
