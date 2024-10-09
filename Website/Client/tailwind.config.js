/** @type {import('tailwindcss').Config} */
const color = require('tailwindcss/colors');

const numbers = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
const numbersFull = [0, 1, 1.5, 2, 2.5, 3, 3.5, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 16, 20, 24, 28, 32, 36, 40, 44, 48, 52, 56, 60, 64, 72, 80, 96];

const sizes = ["none","sm", "md", "lg", "xl", "2xl", "3xl", "full"]

const customColors = {
    sky: color.sky["400"],
    skyHover: color.sky["500"],
    skyFocus: color.sky["600"],
    red: color.red["500"],
    lightRed: color.red["300"],
    green: color.green["500"],
    lightGreen: color.green["300"],
    orange: color.orange["500"],
    blue: "#3273F6",
    blueHover: "#265BC1",
    white: "#ffffff",
    black: "#000000"
};

const generateColorClasses = (colorName) => [
    `bg-${colorName}`,
    `text-${colorName}`,
    `border-${colorName}`,
    `border-t-${colorName}`,
    `divide-${colorName}`,
    `ring-${colorName}`,
    `hover:bg-${colorName}`,
    `hover:border-${colorName}`,
    `hover:ring-${colorName}`,
    `hover:text-${colorName}`,
    `focus:bg-${colorName}`,
    `focus:border-${colorName}`,
    `focus:ring-${colorName}`,
    `focus:text-${colorName}`,
    `active:bg-${colorName}`,
    `active:border-${colorName}`,
    `active:ring-${colorName}`,
    `active:text-${colorName}`,
];

const generateBorderSize = (integers) => [
    `border-${integers}`
]

const generateBorderRadius = (strings) => [
    `rounded-${strings}`
]

const generatePaddingAndMargin = (integers) => [
    `m-${integers}`,
    `mt-${integers}`,
    `mr-${integers}`,
    `mb-${integers}`,
    `ml-${integers}`,
    `p-${integers}`,
    `pt-${integers}`,
    `pr-${integers}`,
    `pb-${integers}`,
    `pl-${integers}`,
    `px-${integers}`,
    `py-${integers}`,
] 

const generateRingClassse = (integers) => [
    `ring-${integers}`,
    `hover:ring-${integers}`,
    `focus:ring-${integers}`,
    `active:ring-${integers}`,
]

const generateDistanceClasses = (integers) => [
    `w-${integers}`,
    `h-${integers}`,
]

module.exports = {
    important: true,
    content: ['./**/*.{razor,html}'],
    theme: {
        extend: {
            colors: {
                ...customColors
            }
        }
    },
    variants: {
        extend: {},
    },
    plugins: [],
    safelist: [
        ...Object.keys(customColors).flatMap(colorName => generateColorClasses(colorName)),
        ...numbers.flatMap(number => generateBorderSize(number)),
        ...sizes.flatMap(size => generateBorderRadius(size)),
        ...numbers.flatMap(number => generatePaddingAndMargin(number)),
        ...numbersFull.flatMap(number => generateDistanceClasses(number)),
        "sm:text-[10px]", "md:text-[12px]", "lg:text-[14px]", "xl:text-[16px]",
        "inline-block"
    ]
}

