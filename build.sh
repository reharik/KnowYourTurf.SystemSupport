##   SET PROJECT="build.proj "
##   SET BUILDARGS=

##   IF  NOT "%1"  == ""          SET BUILDARGS= /target:%1
##   ECHO Running:  %COMMAND_PATH% build.proj  %BUILDARGS%
##   %COMMAND_PATH% build.proj  %BUILDARGS%
##   pause

function getValidComment {

	read -p "Enter your comment for the commit: " comment
	strLen="`expr length "$comment"`"
	while [ $strLen -lt 5 ]; do
		echo "You are evil! Enter a descriptive comment to play nice with others! (control+C to exit)";
		read -p "Enter your comment for the commit: " comment
		strLen="`expr length "$comment"`"
	done;
	returnComment="$comment"; ##return value

}

## getValidComment; #calls function

strMSBuildPath="/c/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe"

sProject="build.proj "

echo "Starting to execute:   $strMSBuildPath $sProject  $*"
sleep 3

$strMSBuildPath $sProject  $*

echo "Finished executing:   $strMSBuildPath $sProject  $*"

exit;


