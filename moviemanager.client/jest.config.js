export default {
    preset: 'ts-jest',
    testEnvironment: 'jest-environment-jsdom',
    transform: {
        "^.+\\.tsx?$": "ts-jest",
        "^.+\\.jsx?$": "babel-jest" // Add this line to process `.jsx` files with `babel-jest`
    },
    moduleNameMapper: {
        '\\.(css|less)$': 'identity-obj-proxy', // Add this line to mock CSS imports
        '\\.(gif|ttf|eot|svg|png)$': '<rootDir>/test/__mocks__/fileMock.js',
    },
    setupFilesAfterEnv: ['<rootDir>/setupTests.js'], // Add this line to specify the setup file
};