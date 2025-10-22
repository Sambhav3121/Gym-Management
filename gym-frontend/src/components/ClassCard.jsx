import React from "react";

export default function ClassCard({
  title,
  trainer,
  when,
  duration,
  location,
  onAction,
}) {
  return (
    <div className="rounded-2xl border p-5 bg-white shadow-sm">
      <div className="flex items-start justify-between">
        <div>
          <h3 className="text-lg font-semibold">{title}</h3>
          <p className="text-sm text-gray-500">Trainer: {trainer || "—"}</p>
        </div>
        {location && (
          <span className="rounded-full bg-gray-100 px-3 py-1 text-xs text-gray-700">
            {location}
          </span>
        )}
      </div>

      <div className="mt-4 grid grid-cols-1 gap-4 sm:grid-cols-3">
        <div>
          <div className="text-xs text-gray-500">Schedule</div>
          <div className="font-medium">{when}</div>
        </div>
        <div>
          <div className="text-xs text-gray-500">Duration</div>
          <div className="font-medium">{duration || "—"}</div>
        </div>
        <div className="sm:text-right">
          <button
            onClick={onAction}
            className="rounded-xl bg-orange-500 px-4 py-2 text-sm font-semibold text-white hover:bg-orange-600"
          >
            View
          </button>
        </div>
      </div>
    </div>
  );
}
