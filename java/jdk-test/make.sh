#! /bin/bash

cd src
javac -d ../bin -cp ../lib/lame.jar run/scan.java
cd ../bin
java -classpath ../lib/lame.jar: -Djava.library.path=./ run.scan $1

