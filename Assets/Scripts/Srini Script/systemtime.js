function OnGUI() {

   var timeString = System.DateTime.Now.ToString("hh:mm:ss"); 

   var dateString = System.DateTime.Now.ToString("MM/dd/yyyy");

   var timeRect = new Rect(500,10,100,20);

   var dateRect = new Rect(500,30,100,20);
   
   GUI.Box(new Rect(490,9,100,50),"");

   GUI.Label(timeRect, timeString);

   GUI.Label(dateRect, dateString);
   
   
   }