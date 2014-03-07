class BulkChangeMeshCompression extends ScriptableWizard {

    @MenuItem ("Assets/Bulk Change Mesh Compression")

    static function Init() {

        ScriptableWizard.DisplayWizard.<BulkChangeMeshCompression>(

        "Bulk Change Mesh Compression", "Bulk Change");

    }

 

    var fileNames = "*.blend";

    var from : ModelImporterMeshCompression;

    var to : ModelImporterMeshCompression;

 

    function OnWizardUpdate ()

    {

        helpString = "Note that this will reimport all changed assets, which may take quite a while.";

        isValid = from != to;

    }

 

    function OnWizardCreate ()

    {

        DirSearch("Assets");

    }

 

    function DirSearch(dir : String)

    {

        var fs = System.IO.Directory.GetFiles(dir, fileNames);

        for (var f in fs) {

            var importer = AssetImporter.GetAtPath(f) as ModelImporter;

            if (importer) {

                if (importer.meshCompression == from) {

                    importer.meshCompression = to;

                    AssetDatabase.ImportAsset(f);

                    Debug.Log("Changed "+f);

                }

            }

        }

        var ds = System.IO.Directory.GetDirectories(dir);

        for (var d in ds)

            DirSearch(d);

    }

}