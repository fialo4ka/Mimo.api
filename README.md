This is a MIMO api app

In first run app will create SQLite db. it will be stored in ".\Mimo.api\Mimo.api\mimo.db" and contain testing data.
For this is used MimoDbContextHelper, if it will give exception. Please delete ".\Mimo.api\Mimo.api\mimo.db" and rerun app.

For changing generated test data quantity change MimoDbContextHelper file.

To alternate base test data, please use this

``
        private static List<Achievement> AchievementList;
        private static List<string> CourseName;
        private static List<string> ChapterName;  
        private static List<string> UserGuid
		
``

To change the amount of Users results, please look to comments in code