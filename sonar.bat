SonarQube.Scanner.MSBuild begin /k:"BrewDay" /d:sonar.exclusions="**/vendor/**/*" /d:sonar.web.file.suffixes=".html,.cshtml" /d:sonar.coverage.exclusions="**/*" /v:"1.0.0"
rem /o:"unimib-brewday-1718" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login="e18c52a2a71eeacd594649c54bc5ef81c958caab"
MSBuild /t:Rebuild
SonarQube.Scanner.MSBuild end
rem /d:sonar.login="e18c52a2a71eeacd594649c54bc5ef81c958caab"
exit