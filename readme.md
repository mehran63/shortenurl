# ShortenURL

This is a very simple app which provides URL shortening service to demonstrate developer skills. 

## Technology and Architecture
The app developed using .Net Core 2 based on Serverless architecture for AWS. The app includes two APIs each backed with a AWS Lambda, ShortenURL api to create a short URL for a provided long URL and Fetch API to redirect a short URL request to the corresponding URL. 

## Live instance
A simple UI hosted at <https://shorturlapp.s3-ap-southeast-2.amazonaws.com/index.html> which communicate with live instance of API available at _https://4ylb1evqt9.execute-api.ap-southeast-2.amazonaws.com/Prod/shorten/_ and _https://4ylb1evqt9.execute-api.ap-southeast-2.amazonaws.com/Prod/fetch/key_

## Development
### Requirements

* AWS CLI already configured with Administrator permission
* [Docker installed](https://www.docker.com/community-edition)
* [SAM CLI installed](https://github.com/awslabs/aws-sam-cli)
* [.NET Core installed](https://www.microsoft.com/net/download)

### Building

```bash
sam build
```

### Packaging and deployment

First and foremost, we need an `S3 bucket` where we can upload our Lambda functions packaged as ZIP before we deploy anything, this is a one-off step:

```bash
aws s3 mb s3://mysamapps
```

Next, run the following command to package Lambda functions to S3:

```bash
sam package --output-template-file packaged.yaml --s3-bucket mysamapps
```

Next, the following command will create a Cloudformation Stack and deploy SAM resources.

```bash
sam deploy --template-file packaged.yaml --stack-name shorten-url --capabilities CAPABILITY_IAM
```

After deployment is complete you can run the following command to retrieve the API Gateway Endpoint URLs:
```bash
aws cloudformation describe-stacks --stack-name shorten-url --query Stacks[].Outputs
```

### Testing

For testing our code, you can use `dotnet test` to run tests defined under `test/`

```bash
dotnet test src\Tests\ShortenUrlTests\ShortenUrlTests.csproj
```