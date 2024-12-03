echo "Switching to branch master"
git checkout master

echo "Building app..."
npm run build

echo "Deploying file to server..."
scp -r build/* kendz@103.82.25.49:/var/www/103.82.25.49/

echo "Done!"