#!/bin/bash
module use /var/remote/projects/software/modules/sets
module load kvl-development nodejs
npm install

node node_modules/quasar-cli/bin/quasar build