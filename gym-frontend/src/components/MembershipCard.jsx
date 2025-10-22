import React from "react";

export default function MembershipCard({
  title,
  subtitle,
  priceDisplay,
  durationLabel,
  features = [],
  badge,
  isCurrent,
  onPurchase,
}) {
  return (
    <div className={`relative rounded-2xl border p-6 shadow-sm bg-white`}>
      {badge && (
        <span className="absolute -top-3 right-4 rounded-full border px-3 py-1 text-xs font-semibold text-green-700 bg-green-100">
          {badge}
        </span>
      )}
      <h3 className="text-xl font-semibold">{title}</h3>
      {subtitle && <p className="mt-1 text-sm text-gray-500">{subtitle}</p>}

      <div className="mt-6">
        <div className="text-4xl font-bold">{priceDisplay}</div>
        <div className="text-sm text-gray-500">{durationLabel}</div>
      </div>

      <ul className="mt-6 space-y-2 text-sm">
        {features.map((f, i) => (
          <li key={i} className="flex items-center gap-2">
            <span className="inline-block h-2 w-2 rounded-full bg-green-500" />
            {f}
          </li>
        ))}
      </ul>

      <button
        disabled={isCurrent}
        onClick={onPurchase}
        className={`mt-6 w-full rounded-xl px-4 py-3 text-sm font-semibold ${
          isCurrent
            ? "bg-gray-200 text-gray-500 cursor-not-allowed"
            : "bg-orange-500 text-white hover:bg-orange-600"
        }`}
      >
        {isCurrent ? "Current Plan" : "Purchase Plan"}
      </button>
    </div>
  );
}
