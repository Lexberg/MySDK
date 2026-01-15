# Render Deployment Guide

This file contains instructions for deploying the Vinmonopolet SDK to Render.

## Prerequisites

1. A Render account (https://render.com)
2. A Vinmonopolet API key from https://apis.vinmonopolet.no

## Deployment Steps

### Option 1: Deploy from GitHub (Recommended)

1. Push your code to GitHub (already done)
2. Log in to Render
3. Click "New +" and select "Web Service"
4. Connect your GitHub repository: `Lexberg/MySDK`
5. Configure the service:
   - **Name**: `vinmonopolet-sdk` (or your preferred name)
   - **Environment**: `Docker`
   - **Region**: Choose closest to your users
   - **Branch**: `master`
   - **Root Directory**: Leave blank (uses repository root)

6. Add environment variables:
   - `VINMONOPOLET_API_KEY` = your API key from Vinmonopolet

7. Click "Create Web Service"

### Option 2: Manual Docker Deployment

1. Build the Docker image locally:
   ```bash
   docker build -t vinmonopolet-sdk .
   ```

2. Test locally:
   ```bash
   docker run -p 8080:8080 -e MySDK__ApiKey=your-api-key vinmonopolet-sdk
   ```

3. Push to a container registry and deploy to Render

## Environment Variables

Configure these in the Render dashboard:

- `ASPNETCORE_ENVIRONMENT` - Set to `Production`
- `MySDK__BaseUrl` - Set to `https://apis.vinmonopolet.no`
- `MySDK__ApiKey` - Your Vinmonopolet API subscription key

## Health Check

Render will automatically monitor your service. The service runs on port 8080.

## Scaling

Render allows you to scale your service:
- Free tier: Limited resources
- Starter: $7/month
- Standard: $25/month and up

## Troubleshooting

### Build fails
- Check that Dockerfile is in the repository root
- Verify all dependencies are in MySDK.csproj

### Service won't start
- Check environment variables are set correctly
- Review logs in Render dashboard

### API calls fail
- Verify API key is correct
- Check Vinmonopolet API status
