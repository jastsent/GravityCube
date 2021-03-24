namespace JastSent {

	public static class ScreenSize {
		
		public static float width{
			get { return MainCamera._camera.orthographicSize * MainCamera._camera.aspect * 2;}
		}
		public static float height{
			get { return MainCamera._camera.orthographicSize * 2;}
		}

		public static float right{
			get { return MainCamera._camera.transform.position.x + width/2;}
		}
		public static float left{
			get { return MainCamera._camera.transform.position.x - width/2;}
		}
		public static float up{
			get { return MainCamera._camera.transform.position.y + height/2;}
		}
		public static float down{
			get { return MainCamera._camera.transform.position.y - height/2;}
		}

		public static float halfaspect{
			get { return 1 - (1-MainCamera._camera.aspect)/2;}
		}
	}
}
