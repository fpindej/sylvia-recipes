#!/bin/bash

#══════════════════════════════════════════════════════════════════════════════
#  Release Tag Creator
#══════════════════════════════════════════════════════════════════════════════
#
#  Usage:
#    ./release.sh backend 1.0.0    # Creates tag: backend-v1.0.0
#    ./release.sh frontend 1.0.0   # Creates tag: frontend-v1.0.0
#    ./release.sh backend          # Auto-increment patch version
#    ./release.sh frontend --minor # Auto-increment minor version
#
#══════════════════════════════════════════════════════════════════════════════

set -e

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[0;33m'
CYAN='\033[0;36m'
BOLD='\033[1m'
NC='\033[0m'

show_help() {
    echo "Release Tag Creator"
    echo ""
    echo "Usage:"
    echo "  ./release.sh <target> [version]    Create a release tag"
    echo "  ./release.sh <target> [--patch|--minor|--major]"
    echo ""
    echo "Targets:"
    echo "  backend      Create backend release (backend-vX.Y.Z)"
    echo "  frontend     Create frontend release (frontend-vX.Y.Z)"
    echo ""
    echo "Options:"
    echo "  --patch      Increment patch version (default)"
    echo "  --minor      Increment minor version"
    echo "  --major      Increment major version"
    echo "  -h, --help   Show this help"
    echo ""
    echo "Examples:"
    echo "  ./release.sh backend 1.0.0     # Tag: backend-v1.0.0"
    echo "  ./release.sh frontend          # Auto-increment patch"
    echo "  ./release.sh backend --minor   # Auto-increment minor"
}

get_latest_tag() {
    local prefix=$1
    git tag -l "${prefix}-v*" --sort=-v:refname | head -n 1
}

parse_version() {
    local tag=$1
    local prefix=$2
    echo "${tag#${prefix}-v}"
}

bump_version() {
    local version=$1
    local bump_type=$2
    
    local major minor patch
    IFS='.' read -r major minor patch <<< "$version"
    
    case $bump_type in
        major)
            major=$((major + 1))
            minor=0
            patch=0
            ;;
        minor)
            minor=$((minor + 1))
            patch=0
            ;;
        patch)
            patch=$((patch + 1))
            ;;
    esac
    
    echo "$major.$minor.$patch"
}

# Parse arguments
TARGET=""
VERSION=""
BUMP_TYPE="patch"

while [[ $# -gt 0 ]]; do
    case $1 in
        -h|--help)
            show_help
            exit 0
            ;;
        --patch)
            BUMP_TYPE="patch"
            shift
            ;;
        --minor)
            BUMP_TYPE="minor"
            shift
            ;;
        --major)
            BUMP_TYPE="major"
            shift
            ;;
        backend|frontend)
            TARGET=$1
            shift
            ;;
        *)
            if [[ $1 =~ ^[0-9]+\.[0-9]+\.[0-9]+$ ]]; then
                VERSION=$1
            else
                echo -e "${RED}Unknown argument: $1${NC}"
                exit 1
            fi
            shift
            ;;
    esac
done

# Validate target
if [ -z "$TARGET" ]; then
    echo -e "${RED}Error: Target (backend/frontend) is required${NC}"
    echo ""
    show_help
    exit 1
fi

# Determine version
if [ -z "$VERSION" ]; then
    LATEST_TAG=$(get_latest_tag "$TARGET")
    
    if [ -z "$LATEST_TAG" ]; then
        VERSION="0.1.0"
        echo -e "${YELLOW}No previous ${TARGET} releases found. Starting at v${VERSION}${NC}"
    else
        CURRENT_VERSION=$(parse_version "$LATEST_TAG" "$TARGET")
        VERSION=$(bump_version "$CURRENT_VERSION" "$BUMP_TYPE")
        echo -e "${CYAN}Current version: ${CURRENT_VERSION}${NC}"
        echo -e "${CYAN}New version: ${VERSION} (${BUMP_TYPE} bump)${NC}"
    fi
fi

TAG_NAME="${TARGET}-v${VERSION}"

# Check if tag already exists
if git rev-parse "$TAG_NAME" >/dev/null 2>&1; then
    echo -e "${RED}Error: Tag ${TAG_NAME} already exists${NC}"
    exit 1
fi

# Confirm
echo ""
echo -e "${BOLD}Ready to create release:${NC}"
echo -e "  Target:  ${CYAN}${TARGET}${NC}"
echo -e "  Version: ${CYAN}${VERSION}${NC}"
echo -e "  Tag:     ${CYAN}${TAG_NAME}${NC}"
echo ""
read -p "Create and push this tag? [y/N] " confirm

if [[ ! "$confirm" =~ ^[Yy]$ ]]; then
    echo "Cancelled."
    exit 0
fi

# Create and push tag
echo ""
echo -e "${CYAN}Creating tag ${TAG_NAME}...${NC}"
git tag -a "$TAG_NAME" -m "Release ${TARGET} v${VERSION}"

echo -e "${CYAN}Pushing tag to origin...${NC}"
git push origin "$TAG_NAME"

echo ""
echo -e "${GREEN}✓ Release ${TAG_NAME} created and pushed!${NC}"
echo ""
echo -e "${BOLD}GitHub Actions will now:${NC}"
echo "  1. Build the Docker image"
echo "  2. Push to ghcr.io"
echo "  3. Deploy to the server"
echo ""
echo -e "Watch progress at: ${CYAN}https://github.com/$(git remote get-url origin | sed 's/.*github.com[:/]\(.*\)\.git/\1/')/actions${NC}"
