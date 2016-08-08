namespace L7
{
	public static class IntegerExtensions {
		public static string GetOrdinal(this int value) {
			if (value <= 0) {
				return value.ToString("d");
			}
			switch ((value % 100)) {
				case 11:
				case 12:
				case 13:
					return (value + "th");
			}
			switch ((value % 10)) {
				case 1:
					return (value + "st");

				case 2:
					return (value + "nd");

				case 3:
					return (value + "rd");
			}
			return (value + "th");
		}
	}


}

