set -e
SCRIPTPATH="$( cd "$(dirname "$0")" >/dev/null 2>&1 ; pwd -P )"

echo "Building EI library"

cd $SCRIPTPATH

make -j

echo "Building EI library OK"