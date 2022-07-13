#!/bin/bash

if [ $# -le 0 ]
    then
    cowsay -d "Arguments expected: gatling-test"
    exit 1
fi

gatling.sh -sf user-files -s $1
cd /opt/gatling
tar -czvf gatling.tar.gz results
wget --method PUT --body-file=gatling.tar.gz https://transfer.sh/gatling.tar.gz -O - -nv
